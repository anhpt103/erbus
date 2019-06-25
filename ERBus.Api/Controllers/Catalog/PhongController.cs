using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Catalog.Phong;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/Phong")]
    [Route("{id?}")]
    [Authorize]
    public class PhongController : ApiController
    {
        private IPhongService _service;
        public PhongController(IPhongService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "Phong")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            var data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode)).OrderBy(x => x.MAPHONG).ToList();
            List<ChoiceObject> listData = new List<ChoiceObject>();
            if (data.Count > 0)
            {
                foreach(var row in data)
                {
                    var obj = new ChoiceObject()
                    {
                        VALUE = row.MAPHONG,
                        TEXT = row.MAPHONG + " | " + row.TENPHONG,
                        DESCRIPTION = row.TENPHONG,
                        PARENT = row.TANG != null ? row.TANG.ToString() : "",
                        ID = row.ID,
                        EXTEND_VALUE = row.MALOAIPHONG
                    };
                    listData.Add(obj);
                }
            }
            if (listData.Count > 0)
            {
                result.Data = listData;
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            return Ok(result);
        }

        [Route("GetDataByMaPhong/{maPhongSelected}")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "Phong")]
        public IHttpActionResult GetDataByMaPhong(string maPhongSelected)
        {
            var result = new TransferObj<ChoiceObject>();
            var unitCode = _service.GetCurrentUnitCode();
            var data = _service.Repository.DbSet.FirstOrDefault(x => x.TRANGTHAI == (int)TypeState.USED && x.MAPHONG.Equals(maPhongSelected) && x.UNITCODE.Equals(unitCode));
            result.Status = false;
            if (data != null)
            {
                result.Status = true;
                result.Data = new ChoiceObject()
                {
                    VALUE = data.MAPHONG,
                    TEXT = data.MAPHONG + " | " + data.TENPHONG,
                    DESCRIPTION = data.TENPHONG,
                    PARENT = data.TANG != null ? data.TANG.ToString() : "",
                    EXTEND_VALUE = data.UNITCODE
                };
            }
            return Ok(result);
        }

        [Route("GetStatusAllRoom")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "DatPhong")]
        public IHttpActionResult GetStatusAllRoom()
        {
            var result = new TransferObj<List<PhongViewModel.StatusRoom>>();
            result.Data = _service.GetListStatusRoom(_service.GetCurrentUnitCode(), _service.GetConnectionString());
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

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "Phong")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<PhongViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<PHONG>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new PHONG().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new PHONG().MAPHONG),
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

        [Route("BuildNewCode/{maTang}")]
        [HttpGet]
        public string BuildNewCode(string maTang)
        {
            return _service.BuildCode(maTang);
        }

        [ResponseType(typeof(PHONG))]
        [CustomAuthorize(Method = "THEM", State = "Phong")]
        public async Task<IHttpActionResult> Post (PhongViewModel.Dto instance)
        {
            var result = new TransferObj<PHONG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MAPHONG == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MAPHONG == instance.MAPHONG && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã phòng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MAPHONG = _service.SaveCode(instance.TANG.ToString());
                var data = Mapper.Map<PhongViewModel.Dto, PHONG>(instance);
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
        [CustomAuthorize(Method = "SUA", State = "Phong")]
        public async Task<IHttpActionResult> Put (string id, PHONG instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<PHONG>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var item = _service.Update(instance);
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


        [ResponseType(typeof(PHONG))]
        [CustomAuthorize(Method = "XOA", State = "Phong")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            PHONG instance = await _service.Repository.FindAsync(id);
            if (instance == null)
            {
                return NotFound();
            }
            try
            {
                 _service.Delete(instance.ID);
                int del = await _service.UnitOfWork.SaveAsync();
                if(del > 0)
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