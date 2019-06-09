using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Knowledge.DatPhong;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Http;

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
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<DatPhongViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<DATPHONG>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new DATPHONG().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new DATPHONG().MA_DATPHONG),
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