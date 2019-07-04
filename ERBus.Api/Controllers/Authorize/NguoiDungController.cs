using AutoMapper;
using ERBus.Entity.Database.Authorize;
using ERBus.Service;
using ERBus.Service.Authorize.NguoiDung;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/Authorize/NguoiDung")]
    [Route("{id?}")]
    [Authorize]
    public class NguoiDungController : ApiController
    {
        private INguoiDungService _service;
        public NguoiDungController(INguoiDungService service)
        {
            _service = service;
        }

        [Route("GetSelectData")]
        [HttpGet]
        public IList<ChoiceObject> GetSelectData()
        {
            var data = _service.Repository.DbSet;
            var unitCode = _service.GetCurrentUnitCode();
            return data.Where(x => x.UNITCODE == unitCode).Select(x => new ChoiceObject() { VALUE = x.MANHANVIEN, TEXT = x.TENNHANVIEN, ID = x.ID }).ToList();
        }

        [Route("GetDetails/{id}")]
        [HttpGet]
        public NGUOIDUNG GetDetails(string id)
        {
            var data = _service.Repository.DbSet;
            var unitCode = _service.GetCurrentUnitCode();
            return data.FirstOrDefault(x => x.ID.Equals(id) && x.UNITCODE == unitCode);
        }

        [Route("CheckExistsUsername/{userName}")]
        [HttpGet]
        public bool CheckExistsUsername(string userName)
        {
            var result = false;
            if (!string.IsNullOrEmpty(userName))
            {
                var unitCode = _service.GetCurrentUnitCode();
                var data = _service.Repository.DbSet.FirstOrDefault(x=>x.USERNAME.Equals(userName.Trim()));
                if (data != null) result = true;
            } else result = true;
            return result;
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "NguoiDung")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<NguoiDungViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<NGUOIDUNG>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new NGUOIDUNG().UNITCODE),
                    Method = FilterMethod.StartsWith,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new NGUOIDUNG().USERNAME),
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

        [ResponseType(typeof(NGUOIDUNG))]
        [CustomAuthorize(Method = "THEM", State = "NguoiDung")]
        public async Task<IHttpActionResult> Post(NguoiDungViewModel.Dto instance)
        {
            var result = new TransferObj<NGUOIDUNG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MANHANVIEN == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MANHANVIEN == instance.MANHANVIEN && x.UNITCODE.StartsWith(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã người dùng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MANHANVIEN = _service.SaveCode();
                instance.I_STATE = "C";
                instance.PASSWORD = MD5Encrypt.MD5Hash(instance.PASSWORD);
                var data = Mapper.Map<NguoiDungViewModel.Dto, NGUOIDUNG>(instance);
                if (!string.IsNullOrEmpty(instance.UNITCODE) && instance.UNITCODE.Equals(curentUnitCode))
                {
                    data.PARENT_UNITCODE = curentUnitCode;
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
                else
                {
                    data.PARENT_UNITCODE = _service.GetParentUnitCode(data.UNITCODE);
                    var item = _service.Insert(data, false);
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
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;

            }
            return Ok(result);
        }


        [ResponseType(typeof(void))]
        [CustomAuthorize(Method = "SUA", State = "NguoiDung")]
        public async Task<IHttpActionResult> Put(string id, NGUOIDUNG instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<NGUOIDUNG>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                instance.I_STATE = "U";
                if (!string.IsNullOrEmpty(instance.UNITCODE) && instance.UNITCODE.Equals(curentUnitCode))
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
                else
                {
                    instance.PARENT_UNITCODE = _service.GetParentUnitCode(instance.UNITCODE);
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
                
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Status = false;
                result.Message = e.Message;
            }
            return Ok(result);
        }


        [ResponseType(typeof(NGUOIDUNG))]
        [CustomAuthorize(Method = "XOA", State = "NguoiDung")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            NGUOIDUNG instance = await _service.Repository.FindAsync(id);
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