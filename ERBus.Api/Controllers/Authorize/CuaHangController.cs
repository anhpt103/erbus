using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Authorize.CuaHang;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/Authorize/CuaHang")]
    [Route("{id?}")]
    [Authorize]
    public class CuaHangController : ApiController
    {
        private ICuaHangService _service;
        public CuaHangController(ICuaHangService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "ThongTinCuaHang")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.MA_CUAHANG.Equals(unitCode)).OrderBy(x => x.MA_CUAHANG).Select(x => new ChoiceObject { VALUE = x.MA_CUAHANG, TEXT = x.MA_CUAHANG + " | " + x.TEN_CUAHANG, DESCRIPTION = x.TEN_CUAHANG, EXTEND_VALUE = x.DIACHI, ID = x.ID }).ToList();
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
        [CustomAuthorize(Method = "XEM", State = "ThongTinCuaHang")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<CuaHangViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<CUAHANG>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new CUAHANG().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new CUAHANG().MA_CUAHANG),
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

        [ResponseType(typeof(CUAHANG))]
        [CustomAuthorize(Method = "THEM", State = "ThongTinCuaHang")]
        public async Task<IHttpActionResult> Post (CuaHangViewModel.Dto instance)
        {
            var result = new TransferObj<CUAHANG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MA_CUAHANG == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MA_CUAHANG == instance.MA_CUAHANG && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã kệ hàng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MA_CUAHANG = _service.SaveCode();
                var data = Mapper.Map<CuaHangViewModel.Dto, CUAHANG>(instance);
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
        [CustomAuthorize(Method = "SUA", State = "ThongTinCuaHang")]
        public async Task<IHttpActionResult> Put (string id, CUAHANG instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<CUAHANG>();
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


        [ResponseType(typeof(CUAHANG))]
        [CustomAuthorize(Method = "XOA", State = "ThongTinCuaHang")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            CUAHANG instance = await _service.Repository.FindAsync(id);
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