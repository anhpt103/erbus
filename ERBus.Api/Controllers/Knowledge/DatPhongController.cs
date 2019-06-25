using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.Knowledge.DatPhong;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Knowledge
{
    [RoutePrefix("api/Knowledge/DatPhong")]
    [Route("{id?}")]
    [Authorize]
    public class DatPhongController : ApiController
    {
        private IDatPhongService _service;
        public DatPhongController(IDatPhongService service)
        {
            _service = service;
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "DatPhong")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<DatPhongViewModel.ViewModel>> result = new TransferObj<PagedObj<DatPhongViewModel.ViewModel>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<DatPhongViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<DatPhongViewModel.ViewModel>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                PagedObj<DatPhongViewModel.ViewModel> tempData = _service.QueryBookingRoom(_service.GetConnectionString(), paged, filtered.AdvanceData.MAPHONG, unitCode);
                result.Data = tempData;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("GetListBookingRoom")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "DatPhong")]
        public IHttpActionResult GetListBookingRoom()
        {
            var result = new TransferObj<List<DatPhongViewModel.DatPhongPayDto>>();
            result.Data = _service.GetListCloseBookingRoom(_service.GetCurrentUnitCode(), _service.GetConnectionString());
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

        [Route("BuildNewCode")]
        [HttpGet]
        public string BuildNewCode()
        {
            return _service.BuildCode();
        }

        [ResponseType(typeof(DATPHONG))]
        [CustomAuthorize(Method = "THEM", State = "DatPhong")]
        public async Task<IHttpActionResult> Post(DatPhongViewModel.Dto instance)
        {
            var result = new TransferObj<DATPHONG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MA_DATPHONG == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MA_DATPHONG == instance.MA_DATPHONG && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại phiếu đặt phòng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MA_DATPHONG = _service.SaveCode();
                instance.TRANGTHAI = 10;
                var data = Mapper.Map<DatPhongViewModel.Dto, DATPHONG>(instance);
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
        [CustomAuthorize(Method = "SUA", State = "DatPhong")]
        public async Task<IHttpActionResult> Put(string id, DATPHONG instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<DATPHONG>();
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
                    result.Message = "Cập nhật giờ đặt thành công";
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

        [ResponseType(typeof(DATPHONG))]
        [CustomAuthorize(Method = "XOA", State = "DatPhong")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            DATPHONG instance = await _service.Repository.FindAsync(id);
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
                    result.Message = "Hủy đặt thành công";
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