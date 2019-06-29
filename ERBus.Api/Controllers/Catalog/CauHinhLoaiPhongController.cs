using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Catalog.CauHinhLoaiPhong;
using System;
using System.Linq;
using System.Threading.Tasks;
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