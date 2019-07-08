﻿using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Catalog.Thue;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/Thue")]
    [Route("{id?}")]
    [Authorize]
    public class ThueController : ApiController
    {
        private IThueService _service;
        public ThueController(IThueService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            string ParenUnitCode = _service.GetParentUnitCode(unitCode);
            string UnitCodeParam = string.IsNullOrEmpty(ParenUnitCode) ? unitCode : ParenUnitCode;
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.StartsWith(UnitCodeParam)).OrderBy(x => x.MATHUE).Select(x => new ChoiceObject { VALUE = x.MATHUE, TEXT = x.MATHUE + " | " + x.TENTHUE, DESCRIPTION = x.TENTHUE, GIATRI = x.GIATRI, ID = x.ID }).ToList();
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

        [Route("GetDataByMaThue/{maThueSelected}")]
        [HttpGet]
        public IHttpActionResult GetDataByMaThue(string maThueSelected)
        {
            var result = new TransferObj<ChoiceObject>();
            var unitCode = _service.GetCurrentUnitCode();
            string ParenUnitCode = _service.GetParentUnitCode(unitCode);
            string UnitCodeParam = string.IsNullOrEmpty(ParenUnitCode) ? unitCode : ParenUnitCode;
            var data = _service.Repository.DbSet.FirstOrDefault(x => x.TRANGTHAI == (int)TypeState.USED && x.MATHUE.Equals(maThueSelected) && x.UNITCODE.StartsWith(UnitCodeParam));
            result.Status = false;
            if (data != null)
            {
                result.Status = true;
                result.Data = new ChoiceObject()
                {
                    VALUE = data.MATHUE,
                    TEXT = data.MATHUE + " | " + data.TENTHUE,
                    DESCRIPTION = data.TENTHUE,
                    GIATRI = data.GIATRI,
                    EXTEND_VALUE = data.UNITCODE
                };
            }
            return Ok(result);
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "Thue")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<ThueViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<THUE>>();
            var unitCode = string.IsNullOrEmpty(filtered.PARENT_UNITCODE) ? filtered.UNITCODE : filtered.PARENT_UNITCODE;
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new THUE().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new THUE().MATHUE),
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

        [ResponseType(typeof(THUE))]
        [CustomAuthorize(Method = "THEM", State = "Thue")]
        public async Task<IHttpActionResult> Post (ThueViewModel.Dto instance)
        {
            var result = new TransferObj<THUE>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MATHUE == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MATHUE == instance.MATHUE && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã thuế này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MATHUE = _service.SaveCode();
                var data = Mapper.Map<ThueViewModel.Dto, THUE>(instance);
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
        [CustomAuthorize(Method = "SUA", State = "Thue")]
        public async Task<IHttpActionResult> Put (string id, THUE instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<THUE>();
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


        [ResponseType(typeof(THUE))]
        [CustomAuthorize(Method = "XOA", State = "Thue")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            THUE instance = await _service.Repository.FindAsync(id);
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