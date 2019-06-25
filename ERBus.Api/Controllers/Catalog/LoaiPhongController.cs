using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Catalog.LoaiPhong;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/LoaiPhong")]
    [Route("{id?}")]
    [Authorize]
    public class LoaiPhongController : ApiController
    {
        private ILoaiPhongService _service;
        public LoaiPhongController(ILoaiPhongService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "LoaiPhong")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode)).OrderBy(x => x.MALOAIPHONG).Select(x => new ChoiceObject { VALUE = x.MALOAIPHONG, TEXT = x.MALOAIPHONG + " | " + x.TENLOAIPHONG, DESCRIPTION = x.TENLOAIPHONG, EXTEND_VALUE = x.MABOHANG, ID = x.ID }).ToList();
            if (result.Data.Count > 0)
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            return Ok(result);
        }

        [Route("GetDetails/{maLoaiPhong}")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "LoaiPhong")]
        public IHttpActionResult GetDetails(string maLoaiPhong)
        {
            var result = new TransferObj<LoaiPhongViewModel.DtoCauHinh>();
            if (!string.IsNullOrEmpty(maLoaiPhong))
            {
                var unitCode = _service.GetCurrentUnitCode();
                var loaiPhong = _service.Repository.DbSet.FirstOrDefault(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode) && x.MALOAIPHONG.Equals(maLoaiPhong));
                if (loaiPhong != null)
                {
                    var data = Mapper.Map<LOAIPHONG, LoaiPhongViewModel.DtoCauHinh>(loaiPhong);
                    var cauHinhLoaiPhong = _service.UnitOfWork.Repository<CAUHINH_LOAIPHONG>().DbSet.FirstOrDefault(x => x.MALOAIPHONG.Equals(data.MALOAIPHONG) && x.UNITCODE.Equals(unitCode));
                    if (cauHinhLoaiPhong != null)
                    {
                        data.MAHANG = cauHinhLoaiPhong.MAHANG;
                        data.SOPHUT = cauHinhLoaiPhong.SOPHUT;
                    }
                    result.Data = data;
                    result.Status = true;
                }
                else
                {
                    result.Data = null;
                    result.Status = false;
                }
            }
            else
            {
                result.Data = null;
                result.Status = false;
            }
            return Ok(result);
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "LoaiPhong")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<LoaiPhongViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<LOAIPHONG>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new LOAIPHONG().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new LOAIPHONG().MALOAIPHONG),
                        Method = OrderMethod.ASC
                    }
                }
            };
            try
            {
                var filterResult = _service.Filter(filtered, query);
                if (!filterResult.WasSuccessful)
                {
                    return NotFound();
                }
                result.Data = filterResult.Value;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("BuildNewCode")]
        [HttpGet]
        public string BuildNewCode()
        {
            return _service.BuildCode();
        }

        [ResponseType(typeof(LOAIPHONG))]
        [CustomAuthorize(Method = "THEM", State = "LoaiPhong")]
        public async Task<IHttpActionResult> Post(LoaiPhongViewModel.Dto instance)
        {
            var result = new TransferObj<LOAIPHONG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MALOAIPHONG == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MALOAIPHONG == instance.MALOAIPHONG && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại loại phòng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MALOAIPHONG = _service.SaveCode();
                var data = Mapper.Map<LoaiPhongViewModel.Dto, LOAIPHONG>(instance);
                string path = _service.PhysicalPathUploadLoaiPhong() + data.MALOAIPHONG + "\\";
                if (!string.IsNullOrEmpty(instance.BACKGROUND_NAME))
                {
                    FileStream fs = new FileStream(path + instance.BACKGROUND_NAME, FileMode.Open, FileAccess.Read);
                    byte[] BackgroundData = new byte[fs.Length];
                    fs.Read(BackgroundData, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    data.BACKGROUND = BackgroundData;
                }

                if (!string.IsNullOrEmpty(instance.ICON_NAME))
                {
                    FileStream fs = new FileStream(path + instance.ICON_NAME, FileMode.Open, FileAccess.Read);
                    byte[] IconData = new byte[fs.Length];
                    fs.Read(IconData, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    data.ICON = IconData;
                }
                var item = _service.Insert(data);
                int inst = await _service.UnitOfWork.SaveAsync();
                if (inst > 0)
                {
                    result.Status = true;
                    result.Data = item;
                    result.Message = "Thêm mới thành công";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Thao tác không thành công";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;

            }
            return Ok(result);
        }


        [ResponseType(typeof(void))]
        [CustomAuthorize(Method = "SUA", State = "LoaiPhong")]
        public async Task<IHttpActionResult> Put(string id, LoaiPhongViewModel.Dto instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<LOAIPHONG>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var data = Mapper.Map<LoaiPhongViewModel.Dto, LOAIPHONG>(instance);
                string path = _service.PhysicalPathUploadLoaiPhong() + data.MALOAIPHONG + "\\";
                if (!string.IsNullOrEmpty(instance.BACKGROUND_NAME))
                {
                    FileStream fs = new FileStream(path + instance.BACKGROUND_NAME, FileMode.Open, FileAccess.Read);
                    byte[] BackgroundData = new byte[fs.Length];
                    fs.Read(BackgroundData, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    data.BACKGROUND = BackgroundData;
                }

                if (!string.IsNullOrEmpty(instance.ICON_NAME))
                {
                    FileStream fs = new FileStream(path + instance.ICON_NAME, FileMode.Open, FileAccess.Read);
                    byte[] IconData = new byte[fs.Length];
                    fs.Read(IconData, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    data.ICON = IconData;
                }
                var item = _service.Update(data);
                int upd = await _service.UnitOfWork.SaveAsync();
                if (upd > 0)
                {
                    result.Status = true;
                    result.Data = item;
                    result.Message = "Cập nhật thành công";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Thao tác không thành công";
                }
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Status = false;
                result.Message = e.Message;
            }
            return Ok(result);
        }


        [ResponseType(typeof(LOAIPHONG))]
        [CustomAuthorize(Method = "XOA", State = "LoaiPhong")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            LOAIPHONG instance = await _service.Repository.FindAsync(id);
            if (instance == null)
            {
                return NotFound();
            }
            try
            {
                _service.Delete(instance.ID);
                int del = await _service.UnitOfWork.SaveAsync();
                if (del > 0)
                {
                    result.Data = true;
                    result.Status = true;
                    result.Message = "Xóa thành công bản ghi";
                }
                else
                {
                    result.Data = false;
                    result.Status = false;
                    result.Message = "Thao tác không thành công";
                }
            }
            catch (Exception e)
            {
                result.Data = false;
                result.Status = false;
                result.Message = e.Message;
            }
            return Ok(result);
        }

        [Route("UploadBackground")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult UploadBackground()
        {
            var result = new TransferObj<LoaiPhongViewModel.InfoUpload>();
            try
            {
                LoaiPhongViewModel.InfoUpload data = _service.UploadImageLoaiPhong();
                if (data != null && !string.IsNullOrEmpty(data.FILENAME))
                {
                    result.Status = true;
                    result.Data = data;
                    result.Message = "Success";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Not Success";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Data = null;
                result.Message = "Exception:" + e.ToString();
            }
            return Ok(result);
        }

        [Route("UploadIcon")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult UploadIcon()
        {
            var result = new TransferObj<LoaiPhongViewModel.InfoUpload>();
            try
            {
                LoaiPhongViewModel.InfoUpload data = _service.UploadImageLoaiPhong();
                if (data != null && !string.IsNullOrEmpty(data.FILENAME))
                {
                    result.Status = true;
                    result.Data = data;
                    result.Message = "Success";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Not Success";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Data = null;
                result.Message = "Exception:" + e.ToString();
            }
            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Repository.DataContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}