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
        [CustomAuthorize(Method = "XEM", State = "CuaHang")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.MA_CUAHANG.StartsWith(unitCode)).OrderBy(x => x.MA_CUAHANG).Select(x => new ChoiceObject { VALUE = x.MA_CUAHANG, TEXT = x.MA_CUAHANG + " | " + x.TEN_CUAHANG, DESCRIPTION = x.TEN_CUAHANG, EXTEND_VALUE = x.DIACHI, ID = x.ID }).ToList();
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

        [Route("GetAllChildren/{maCuaHangCha}")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "CuaHang")]
        public IHttpActionResult GetAllChildren(string maCuaHangCha)
        {
            var result = new TransferObj<List<CUAHANG>>();
            if (!string.IsNullOrEmpty(maCuaHangCha))
            {
                var unitCode = _service.GetCurrentUnitCode();
                result.Data = _service.Repository.DbSet.Where(x => x.MA_CUAHANG_CHA.Equals(maCuaHangCha)).OrderBy(x => x.MA_CUAHANG).ToList();
                if (result.Data.Count > 0)
                {
                    result.Status = true;
                }
                else
                {
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

        [Route("GetAllDataByUniCode/{unitCode}")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "CuaHang")]
        public IHttpActionResult GetAllDataByUniCode(string unitCode)
        {
            var result = new TransferObj<List<ChoiceObject>>();
            if (!string.IsNullOrEmpty(unitCode))
            {
                result.Data = _service.Repository.DbSet.Where(x => x.UNITCODE.StartsWith(unitCode)).OrderBy(x => x.MA_CUAHANG).Select(x => new ChoiceObject { VALUE = x.MA_CUAHANG, TEXT = x.MA_CUAHANG + " | " + x.TEN_CUAHANG, DESCRIPTION = x.TEN_CUAHANG, EXTEND_VALUE = x.DIACHI, ID = x.ID }).ToList();
                if (result.Data.Count > 0)
                {
                    result.Status = true;
                }
                else
                {
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
        [CustomAuthorize(Method = "XEM", State = "CuaHang")]
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

        [Route("BuildNewCodeChildren/{maCuaHang}")]
        [HttpGet]
        public string BuildNewCodeChildren(string maCuaHang)
        {
            return _service.BuildCodeChildren(maCuaHang);
        }

        [Route("Post")]
        [HttpPost]
        [ResponseType(typeof(CUAHANG))]
        [CustomAuthorize(Method = "THEM", State = "CuaHang")]
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
                    result.Message = "Đã tồn tại mã cửa hàng này";
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

        [Route("Update")]
        [HttpPost]
        [CustomAuthorize(Method = "SUA", State = "CuaHang")]
        public async Task<IHttpActionResult> Update(CuaHangViewModel.PARAMOBJ_UPDATE paramObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = new TransferObj();
            if(paramObj.LST_EDIT.Count > 0)
            {
                foreach(var recordEdit in paramObj.LST_EDIT)
                {
                    var recordId = _service.Repository.DbSet.FirstOrDefault(x => x.ID.Equals(recordEdit.ID));
                    if (recordId == null)
                    {
                        result.Status = false;
                        result.Data = false;
                        result.Message = "Mã ID không hợp lệ";
                        return Ok(result);
                    }
                    else
                    {
                        recordId.MA_CUAHANG = recordEdit.MA_CUAHANG;
                        recordId.MA_CUAHANG_CHA = recordEdit.MA_CUAHANG_CHA;
                        recordId.TEN_CUAHANG = recordEdit.TEN_CUAHANG;
                        recordId.SODIENTHOAI = recordEdit.SODIENTHOAI;
                        recordId.DIACHI = recordEdit.DIACHI;
                        recordId.UNITCODE = recordEdit.UNITCODE;
                        recordId.ObjectState = ObjectState.Modified;
                    }
                    _service.Update(recordId, null, null, false);
                }
            }
            if (paramObj.LST_DELETE.Count > 0)
            {
                foreach (var recordDelete in paramObj.LST_DELETE)
                {
                    var recordId = _service.Repository.DbSet.FirstOrDefault(x => x.ID.Equals(recordDelete.ID));
                    if (recordId == null)
                    {
                        result.Status = false;
                        result.Data = false;
                        result.Message = "Mã ID không hợp lệ";
                        return Ok(result);
                    }
                    else
                    {
                        _service.Delete(recordId.ID);
                    }
                }
            }

            if (paramObj.RECORD_ADD != null && !string.IsNullOrEmpty(paramObj.RECORD_ADD.MA_CUAHANG) && !string.IsNullOrEmpty(paramObj.RECORD_ADD.MA_CUAHANG_CHA))
            {
                paramObj.RECORD_ADD.MA_CUAHANG = _service.SaveCodeChildren(paramObj.RECORD_ADD.MA_CUAHANG_CHA);
                var data = Mapper.Map<CuaHangViewModel.Dto, CUAHANG>(paramObj.RECORD_ADD);
                data.UNITCODE = data.MA_CUAHANG;
                _service.Insert(data, false);
            }

            int upd = await _service.UnitOfWork.SaveAsync();
            if (upd > 0)
            {
                result.Status = true;
                result.Data = true;
                result.Message = "Cập nhật thành công";
            }
            else
            {
                result.Status = false;
                result.Data = false;
                result.Message = "Thao tác không thành công";
            }
            return Ok(result);
        }

        [ResponseType(typeof(CUAHANG))]
        [CustomAuthorize(Method = "XOA", State = "CuaHang")]
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