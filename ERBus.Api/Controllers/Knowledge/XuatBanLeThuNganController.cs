using AutoMapper;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.Knowledge.XuatBanLeThuNgan;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ERBus.Api.Controllers.Knowledge
{
    [RoutePrefix("api/Knowledge/XuatBanLeThuNgan")]
    [Route("{id?}")]
    [Authorize]
    public class XuatBanLeThuNganController : ApiController
    {
        private IXuatBanLeThuNganService _service;
        public XuatBanLeThuNganController(IXuatBanLeThuNganService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "XuatBanLeThuNgan")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            //var dayOfTenBefore = DateTime.Now.AddDays(-10);
            //result.Data = _service.Repository.DbSet.Where(x => x.UNITCODE.Equals(unitCode) && x.NGAY_GIAODICH > dayOfTenBefore).OrderByDescending(x => x.NGAY_GIAODICH).Select(x => new ChoiceObject { VALUE = x.MA_GIAODICH, TEXT = x.MA_GIAODICH + " | " + x.LOAI_GIAODICH, DESCRIPTION = x.LOAI_GIAODICH, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
            result.Data = _service.Repository.DbSet.Where(x => x.UNITCODE.Equals(unitCode)).OrderByDescending(x => x.NGAY_GIAODICH).Select(x => new ChoiceObject { VALUE = x.MA_GIAODICH, TEXT = x.MA_GIAODICH , DESCRIPTION = x.LOAI_GIAODICH, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
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
        [CustomAuthorize(Method = "XEM", State = "XuatBanLeThuNgan")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<XuatBanLeThuNganViewModel.View>> result = new TransferObj<PagedObj<XuatBanLeThuNganViewModel.View>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<XuatBanLeThuNganViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<XuatBanLeThuNganViewModel.View>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                PagedObj<XuatBanLeThuNganViewModel.View> tempData = _service.QueryPageGiaoDich(_service.GetConnectionString(), paged, filtered.Summary, unitCode);
                result.Data = tempData;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("GetDetails/{ID}")]
        [HttpGet]
        public IHttpActionResult GetDetails(string ID)
        {
            var result = new TransferObj<XuatBanLeThuNganViewModel.Dto>();
            XuatBanLeThuNganViewModel.Dto dto = new XuatBanLeThuNganViewModel.Dto();
            if (string.IsNullOrEmpty(ID))
            {
                return BadRequest("ID không chính xác");
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var giaoDich = _service.Repository.DbSet.FirstOrDefault(x => x.UNITCODE.Equals(unitCode) && x.ID.Equals(ID));
                if (giaoDich != null)
                {
                    dto = Mapper.Map<GIAODICH, XuatBanLeThuNganViewModel.Dto>(giaoDich);
                    var giaoDichChiTiet = _service.UnitOfWork.Repository<GIAODICH_CHITIET>().DbSet.Where(x => x.MA_GIAODICH.Equals(giaoDich.MA_GIAODICH)).OrderByDescending(x => x.SAPXEP).ToList();
                    dto.DataDetails = Mapper.Map<List<GIAODICH_CHITIET>, List<XuatBanLeThuNganViewModel.DtoDetails>>(giaoDichChiTiet);
                    if (dto.DataDetails.Count > 0)
                    {
                        string listMatHang = "";
                        foreach (var matHang in dto.DataDetails)
                        {
                            listMatHang += matHang.MAHANG + ",";
                        }
                        listMatHang = listMatHang.Substring(0, listMatHang.Length - 1);
                        var MatHangViewModel = _service.GetDataMatHang(_service.ConvertConditionStringToArray(listMatHang), unitCode, _service.GetConnectionString());
                        foreach (var row in dto.DataDetails)
                        {
                            var hang = MatHangViewModel.FirstOrDefault(x => x.MAHANG.Equals(row.MAHANG));
                            if (hang != null)
                            {
                                row.TENHANG = hang.TENHANG;
                                row.MADONVITINH = hang.MADONVITINH;
                            }
                        }
                    }
                }
                if (dto != null && !string.IsNullOrEmpty(dto.MA_GIAODICH))
                {
                    result.Data = dto;
                    result.Status = true;
                    result.Message = "Oke";
                }
                else
                {
                    result.Data = null;
                    result.Status = false;
                    result.Message = "NotFound";
                }
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