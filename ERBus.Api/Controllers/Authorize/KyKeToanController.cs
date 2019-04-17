using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Service;
using ERBus.Service.Authorize.KyKeToan;
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
    [RoutePrefix("api/Authorize/KyKeToan")]
    [Route("{id?}")]
    [Authorize]
    public class KyKeToanController : ApiController
    {
        private IKyKeToanService _service;
        public KyKeToanController(IKyKeToanService service)
        {
            _service = service;
        }
        
        [Route("GetKyKeToan")]
        [HttpGet]
        public IHttpActionResult GetKyKeToan()
        {
            var result = new TransferObj<KYKETOAN>();
            var _unitCode = _service.GetCurrentUnitCode();
            var kyKeToan = _service.Repository.DbSet.Where(x => x.UNITCODE == _unitCode && x.TRANGTHAI == (int)TypeState.APPROVAL);
            if (kyKeToan != null)
            {
                var lastPeriod = kyKeToan.OrderByDescending(x => new { x.NAM, x.KY }).FirstOrDefault();
                if (lastPeriod != null)
                {
                    result.Status = true;
                    result.Data = lastPeriod;
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                }
            }
            return Ok(result);
        }

        [Route("GetLastestPeriod")]
        [HttpGet]
        public IHttpActionResult GetLastestPeriod()
        {
            var result = new TransferObj<KyKeToanViewModel.ViewModel>();
            var _unitCode = _service.GetCurrentUnitCode();
            var kyKeToan = _service.GetLastestPeriod(_unitCode, _service.GetConnectionString());
            if (kyKeToan != null && kyKeToan.NAM > 0)
            {
                kyKeToan.TABLE_NAME = _service.Get_TableName_XNT(kyKeToan.NAM, kyKeToan.KY);
                result.Status = true;
                result.Data = kyKeToan;
            }
            else
            {
                result.Status = false;
                result.Data = null;
            }
            return Ok(result);
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "KyKeToan")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<KyKeToanViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<KYKETOAN>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new KYKETOAN().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new KYKETOAN().KY),
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

        [ResponseType(typeof(KYKETOAN))]
        [CustomAuthorize(Method = "THEM", State = "KyKeToan")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(KyKeToanViewModel.Dto instance)
        {
            var result = new TransferObj<List<KYKETOAN>>();
            List<KYKETOAN> listKyKeToan = new List<KYKETOAN>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.NAM == 0)
            {
                result.Status = false;
                result.Message = "Kỳ kế toán không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.NAM == instance.NAM && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại kỳ kế toán này";
                    return Ok(result);
                }
            }
            try
            {
                var unitCode = _service.GetCurrentUnitCode();
                var startDate = new DateTime(instance.NAM, 1, 1);
                var endDate = new DateTime(instance.NAM, 12, 31);
                int count = 0;
                while (startDate <= endDate)
                {
                    count++;
                    var item = new KYKETOAN()
                    {
                        DENNGAY = startDate.Date,
                        TUNGAY = startDate.Date,
                        KY = count,
                        NAM = startDate.Year,
                        TENKY = string.Format("Kỳ kế toán {0}", count),
                        TRANGTHAI = 0
                    };
                    startDate = startDate.AddDays(1);
                    _service.Insert(item);
                    listKyKeToan.Add(item);
                }
                int inst = await _service.UnitOfWork.SaveAsync();
                if (inst > 0)
                {
                    result.Status = true;
                    result.Data = listKyKeToan;
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

        [Route("ClosingOutPeriod")]
        [HttpPost]
        public async Task<IHttpActionResult> ClosingOutPeriod(KYKETOAN instance)
        {
            var result = new TransferObj<KYKETOAN>();
            var exist = _service.Find(instance);
            if (exist != null)
            {
                if (instance.TRANGTHAI == (int)TypeState.APPROVAL)
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Kỳ này đã được duyệt";
                    return Ok(result);
                }
                var unitCode = _service.GetCurrentUnitCode();
                var exsitPeriodNotClose = _service.Repository.DbSet.Any(x => x.NAM == exist.NAM && x.KY < exist.KY && x.TRANGTHAI != (int)TypeState.APPROVAL && x.UNITCODE.Equals(unitCode));
                if (exsitPeriodNotClose)
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Chưa khóa sổ kỳ trước";
                    return Ok(result);
                }

                var tableName = _service.Get_TableName_XNT(instance.NAM, instance.KY);
                var previousTalbeName = _service.Get_TableName_XNT_Previous(instance.NAM, instance.KY - 1);
                try
                {
                    if (_service.KhoaSoKyKeToan(previousTalbeName, tableName, unitCode, instance.NAM, instance.KY, _service.GetConnectionString()))
                    {
                        exist.TRANGTHAI = (int)TypeState.APPROVAL;
                        exist.ObjectState = ObjectState.Modified;
                        int appro = await _service.UnitOfWork.SaveAsync();
                        if (appro > 0)
                        {
                            result.Data = exist;
                            result.Status = true;
                            result.Message = "Khóa sổ kỳ kế toán [" + exist.KY + "] thành công";
                        }
                        else
                        {
                            result.Data = null;
                            result.Status = false;
                            result.Message = "Khóa sổ không thành công";
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Data = null;
                        result.Message = "Khóa sổ xảy ra lỗi! Không thành công";
                    }

                }
                catch (Exception e)
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Lỗi: " + e.Message;
                }
            }
            return Ok(result);
        }


        [Route("ClosingOutListPeriodNotLock")]
        [HttpPost]
        public IHttpActionResult ClosingOutListPeriodNotLock(List<KyKeToanViewModel.ViewModel> listPeriod)
        {
            var result = new TransferObj<List<KyKeToanViewModel.ViewModel>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                if (_service.KhoaSoNhieuKyKeToan(listPeriod, unitCode, _service.GetConnectionString()))
                {
                    result.Data = listPeriod;
                    result.Status = true;
                    result.Message = "CLOSING_ALLPERIOD_SUCCESS";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "CLOSING_ALLPERIOD_NOTSUCCESS";
                }

            }
            catch (Exception e)
            {
                result.Status = false;
                result.Data = null;
                result.Message = "CLOSING_ALLPERIOD_ERROR";
            }
            return Ok(result);
        }


        [ResponseType(typeof(void))]
        [CustomAuthorize(Method = "SUA", State = "KyKeToan")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string id, KYKETOAN instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<KYKETOAN>();
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