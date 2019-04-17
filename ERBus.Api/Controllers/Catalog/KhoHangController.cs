using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.Catalog.KhoHang;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/KhoHang")]
    [Route("{id?}")]
    [Authorize]
    public class KhoHangController : ApiController
    {
        private IKhoHangService _service;
        public KhoHangController(IKhoHangService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "KhoHang")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode)).OrderBy(x => x.MAKHO).Select(x => new ChoiceObject { VALUE = x.MAKHO, TEXT = x.MAKHO + " | " + x.TENKHO, DESCRIPTION = x.TENKHO, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
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
        [CustomAuthorize(Method = "XEM", State = "KhoHang")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<KHOHANG>> result = new TransferObj<PagedObj<KHOHANG>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<KhoHangViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<KHOHANG>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                PagedObj<KHOHANG> tempData = _service.QueryPageKhoHang(_service.GetConnectionString(), paged, filtered.Summary, unitCode);
                result.Data = tempData;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception ex)
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

        [ResponseType(typeof(KHOHANG))]
        [CustomAuthorize(Method = "THEM", State = "KhoHang")]
        public async Task<IHttpActionResult> Post (KhoHangViewModel.Dto instance)
        {
            var result = new TransferObj<KHOHANG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MAKHO == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MAKHO == instance.MAKHO && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã kho hàng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MAKHO = _service.SaveCode();
                var data = Mapper.Map<KhoHangViewModel.Dto, KHOHANG>(instance);
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
        [CustomAuthorize(Method = "SUA", State = "KhoHang")]
        public async Task<IHttpActionResult> Put (string id, KHOHANG instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<KHOHANG>();
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


        [ResponseType(typeof(KHOHANG))]
        [CustomAuthorize(Method = "XOA", State = "KhoHang")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            KHOHANG instance = await _service.Repository.FindAsync(id);
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