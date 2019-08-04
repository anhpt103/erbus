using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Catalog.CauHinhLoaiPhong;
using ERBus.Service.Catalog.LoaiPhong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/CauHinhLoaiPhong")]
    [Route("{id?}")]
    [Authorize]
    public class CauHinhLoaiPhongController : ApiController
    {
        private ICauHinhLoaiPhongService _service;
        public CauHinhLoaiPhongController(ICauHinhLoaiPhongService service)
        {
            _service = service;
        }

        [Route("Post")]
        [HttpPost]
        [ResponseType(typeof(CAUHINH_LOAIPHONG))]
        public async Task<IHttpActionResult> Post(CauHinhLoaiPhongViewModel.Dto instance)
        {
            var result = new TransferObj<CAUHINH_LOAIPHONG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (string.IsNullOrEmpty(instance.MALOAIPHONG))
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
                    exist.MAHANG = instance.MAHANG;
                    exist.MAHANG_DICHVU = instance.MAHANG_DICHVU;
                    exist.SOPHUT = instance.SOPHUT;
                    exist.ObjectState = ObjectState.Modified;
                    _service.Update(exist);
                    result.Data = exist;
                }
                else
                {
                    var data = Mapper.Map<CauHinhLoaiPhongViewModel.Dto, CAUHINH_LOAIPHONG>(instance);
                    data.ID = Guid.NewGuid().ToString();
                    var item = _service.Insert(data);
                    result.Data = item;
                }
                int action = await _service.UnitOfWork.SaveAsync();
                if (action > 0)
                {
                    result.Status = true;
                    result.Message = "Cập nhật thành công";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Thao tác không thành công";
                }
            }
            return Ok(result);
        }

        [Route("PostCost")]
        [HttpPost]
        [ResponseType(typeof(CAUHINH_LOAIPHONG))]
        public async Task<IHttpActionResult> PostCost(List<LoaiPhongViewModel.DtoCauHinh> listData)
        {
            var result = new TransferObj<List<CAUHINH_LOAIPHONG_GIACA>>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (listData.Count > 0)
            {
                try
                {
                    var data = Mapper.Map<List<LoaiPhongViewModel.DtoCauHinh>, List<CAUHINH_LOAIPHONG_GIACA>>(listData);
                    data.ForEach(x =>
                    {
                        x.ID = Guid.NewGuid().ToString();
                        x.UNITCODE = curentUnitCode;
                        x.I_CREATE_DATE = DateTime.Now;
                        var currentUser = (HttpContext.Current.User as ClaimsPrincipal);
                        x.I_CREATE_BY = currentUser.Identity.Name;
                        var listCauHinh_GiaCa = _service.UnitOfWork.Repository<CAUHINH_LOAIPHONG_GIACA>().DbSet.Where(y => y.MALOAIPHONG == x.MALOAIPHONG && y.MAHANG == x.MAHANG && y.UNITCODE == x.UNITCODE).ToList();
                        if (listCauHinh_GiaCa.Count > 0)
                        {
                            foreach (var row in listCauHinh_GiaCa)
                            {
                                row.ObjectState = ObjectState.Deleted;
                                _service.UnitOfWork.Repository<CAUHINH_LOAIPHONG_GIACA>().Delete(row.ID);
                            }
                        }
                    });
                    _service.UnitOfWork.Repository<CAUHINH_LOAIPHONG_GIACA>().InsertRange(data);
                    int inst = await _service.UnitOfWork.SaveAsync();
                    if (inst > 0)
                    {
                        result.Status = true;
                        result.Data = data;
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
            }
            return Ok(result);
        }

        [Route("GetCostByLoaiPhong/{maLoaiPhong}/{maHang}")]
        [HttpGet]
        public IHttpActionResult GetCostByLoaiPhong(string maLoaiPhong, string maHang)
        {
            var result = new TransferObj<List<CAUHINH_LOAIPHONG_GIACA>>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (!string.IsNullOrEmpty(maLoaiPhong) && !string.IsNullOrEmpty(maHang))
            {
                try
                {
                    var data = _service.UnitOfWork.Repository<CAUHINH_LOAIPHONG_GIACA>().DbSet.Where(x => x.MALOAIPHONG.Equals(maLoaiPhong) && x.MAHANG.Equals(maHang) && x.UNITCODE.Equals(curentUnitCode)).ToList();
                    if (data.Count > 0)
                    {
                        result.Status = true;
                        result.Data = data;
                        result.Message = "Oke";
                    }
                    else
                    {
                        result.Status = false;
                        result.Data = null;
                        result.Message = "NotFound";
                    }
                }
                catch (Exception e)
                {
                    result.Status = false;
                    result.Message = e.Message;
                }
            }
            return Ok(result);
        }

        [Route("GetAllData")]
        [HttpGet]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.UnitOfWork.Repository<CAUHINH_LOAIPHONG_GIACA>().DbSet.Where(x => x.UNITCODE.Equals(unitCode)).Select(x => new ChoiceObject { VALUE = x.MAHANG, TEXT = x.MALOAIPHONG, DESCRIPTION = x.MALOAIPHONG, GIATRI = x.GIABANLE_VAT, ID = x.ID }).ToList();
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