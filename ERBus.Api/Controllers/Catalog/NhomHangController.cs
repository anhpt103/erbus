using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Catalog.NhomHang;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/NhomHang")]
    [Route("{id?}")]
    [Authorize]
    public class NhomHangController : ApiController
    {
        private INhomHangService _service;
        public NhomHangController(INhomHangService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "NhomHang")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            string ParenUnitCode = _service.GetParentUnitCode(unitCode);
            string UnitCodeParam = string.IsNullOrEmpty(ParenUnitCode) ? unitCode : ParenUnitCode;
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.StartsWith(UnitCodeParam)).OrderBy(x => x.MALOAI).Select(x => new ChoiceObject { VALUE = x.MANHOM, TEXT = x.MANHOM + " | " + x.TENNHOM, DESCRIPTION = x.TENNHOM, PARENT = x.MALOAI, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
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
        [Route("GetDataByMaLoai/{maLoaiSelected}")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "NhomHang")]
        public IHttpActionResult GetDataByMaLoai(string maLoaiSelected)
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            string ParenUnitCode = _service.GetParentUnitCode(unitCode);
            string UnitCodeParam = string.IsNullOrEmpty(ParenUnitCode) ? unitCode : ParenUnitCode;
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.MALOAI.Equals(maLoaiSelected) && x.UNITCODE.StartsWith(UnitCodeParam)).OrderBy(x => x.MALOAI).Select(x => new ChoiceObject { VALUE = x.MANHOM, TEXT = x.MANHOM + " | " + x.TENNHOM, DESCRIPTION = x.TENNHOM, PARENT = x.MALOAI, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
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
        [CustomAuthorize(Method = "XEM", State = "NhomHang")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<NHOMHANG>> result = new TransferObj<PagedObj<NHOMHANG>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<NhomHangViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<NHOMHANG>>();
            var unitCode = string.IsNullOrEmpty(filtered.PARENT_UNITCODE) ? filtered.UNITCODE : filtered.PARENT_UNITCODE;
            try
            {
                PagedObj<NHOMHANG> tempData = _service.QueryPageNhomHang(_service.GetConnectionString(), paged, filtered.Summary, unitCode);
                result.Data = tempData;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("BuildNewCode/{UnitCode}")]
        [HttpGet]
        public string BuildNewCode(string UnitCode)
        {
            return _service.BuildCode(UnitCode);
        }

        [ResponseType(typeof(NHOMHANG))]
        [CustomAuthorize(Method = "THEM", State = "NhomHang")]
        public async Task<IHttpActionResult> Post (NhomHangViewModel.Dto instance)
        {
            var result = new TransferObj<NHOMHANG>();
            var CurentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MANHOM == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MANHOM == instance.MANHOM && x.UNITCODE.Equals(CurentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã nhóm này";
                    return Ok(result);
                }
            }
            try
            {
                string ParenUnitCode = _service.GetParentUnitCode(CurentUnitCode);
                string UnitCodeParam = string.IsNullOrEmpty(ParenUnitCode) ? CurentUnitCode : ParenUnitCode;
                instance.MANHOM = _service.SaveCode(UnitCodeParam);
                var data = Mapper.Map<NhomHangViewModel.Dto, NHOMHANG>(instance);
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
        [CustomAuthorize(Method = "SUA", State = "NhomHang")]
        public async Task<IHttpActionResult> Put (string id, NHOMHANG instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<NHOMHANG>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var item = _service.Update(instance, null, null, false);
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


        [ResponseType(typeof(NHOMHANG))]
        [CustomAuthorize(Method = "XOA", State = "NhomHang")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            NHOMHANG instance = await _service.Repository.FindAsync(id);
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