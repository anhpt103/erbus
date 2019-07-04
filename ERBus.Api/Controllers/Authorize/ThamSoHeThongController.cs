using AutoMapper;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Authorize.ThamSoHeThong;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ERBus.Service.Catalog.ThamSoHeThong;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/Authorize/ThamSoHeThong")]
    [Route("{id?}")]
    [Authorize]
    public class ThamSoHeThongController : ApiController
    {
        private IThamSoHeThongService _service;
        public ThamSoHeThongController(IThamSoHeThongService service)
        {
            _service = service;
        }
        [Route("GetDataByMaThamSo")]
        [HttpGet]
        //[CustomAuthorize(Method = "XEM", State = "ThamSoHeThong")]
        public IHttpActionResult GetDataByMaThamSo()
        {
            var result = new TransferObj<List<THAMSOHETHONG>>();
            var unitCode = _service.GetCurrentUnitCode();
            var data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.GIATRI_SO == (int)TypeState.USED && x.UNITCODE.Equals(unitCode)).ToList();
            if(data != null)
            {
                result.Data = data;
                result.Status = true;
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
        [CustomAuthorize(Method = "XEM", State = "ThamSoHeThong")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<ThamSoHeThongViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<THAMSOHETHONG>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    SubFilters = new List<IQueryFilter>()
                    {
                        new QueryFilterLinQ()
                        {
                            Property = ClassHelper.GetProperty(() => new THAMSOHETHONG().UNITCODE),
                            Method = FilterMethod.EqualTo,
                            Value = unitCode
                        },
                        new QueryFilterLinQ()
                        {
                            Property = ClassHelper.GetProperty(() => new THAMSOHETHONG().TRANGTHAI),
                            Method = FilterMethod.EqualTo,
                            Value = 10
                        }
                    },
                    Method = FilterMethod.And
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new THAMSOHETHONG().MA_THAMSO),
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

        [Route("SettingParam")]
        [ResponseType(typeof(THAMSOHETHONG))]
        [CustomAuthorize(Method = "THEM", State = "ThamSoHeThong")]
        public async Task<IHttpActionResult> SettingParam(ThamSoHeThongViewModel.Dto instance)
        {
            var result = new TransferObj<THAMSOHETHONG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            try
            {
                if (instance.MA_THAMSO == "")
                {
                    result.Status = false;
                    result.Message = "Mã không hợp lệ";
                    return Ok(result);
                }
                else
                {
                    var dto = Mapper.Map<ThamSoHeThongViewModel.Dto, THAMSOHETHONG>(instance);
                    var exist = _service.FindById(instance.ID);
                    if (exist != null)
                    {
                        var item = _service.Update(dto);
                        dto.UNITCODE = curentUnitCode;
                        dto.TRANGTHAI = 10;
                        int udp = await _service.UnitOfWork.SaveAsync();
                        if (udp > 0)
                        {
                            result.Status = true;
                            result.Data = item;
                            result.Message = "Cập nhật thành công";
                        }
                        else
                        {
                            result.Status = false;
                            result.Data = null;
                            result.Message = "Cập nhật không thành công";
                        }
                    }
                    else
                    {
                        _service.Insert(dto);
                        int inst = await _service.UnitOfWork.SaveAsync();
                        if (inst > 0)
                        {
                            result.Status = true;
                            result.Data = dto;
                            result.Message = "Cài đặt tham số thành công";
                        }
                        else
                        {
                            result.Status = false;
                            result.Data = null;
                            result.Message = "Cài đặt tham số không thành công";
                        }
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