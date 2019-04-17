using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Knowledge.XuatBan;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static ERBus.Service.Knowledge.XuatBan.XuatBanViewModel;

namespace ERBus.Api.Controllers.Knowledge
{
    [RoutePrefix("api/Knowledge/XuatBan")]
    [Route("{id?}")]
    [Authorize]
    public class XuatBanController : ApiController
    {
        private IXuatBanService _service;
        public XuatBanController(IXuatBanService service)
        {
            _service = service;
        }
        

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "XuatBan")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<CHUNGTU>> result = new TransferObj<PagedObj<CHUNGTU>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<CHUNGTU>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                PagedObj<CHUNGTU> tempData = _service.QueryPageChungTu(_service.GetConnectionString(), paged, filtered.Summary, unitCode);
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

        [ResponseType(typeof(CHUNGTU))]
        [CustomAuthorize(Method = "THEM", State = "XuatBan")]
        public async Task<IHttpActionResult> Post (Dto instance)
        {
            var result = new TransferObj<CHUNGTU>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MA_CHUNGTU == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MA_CHUNGTU == instance.MA_CHUNGTU && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại chứng từ này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MA_CHUNGTU = _service.SaveCode();
                var item = _service.InsertChungTu(instance);
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

        [Route("GetDetails/{ID}")]
        [HttpGet]
        public IHttpActionResult GetDetails(string ID)
        {
            var result = new TransferObj<Dto>();
            Dto dto = new Dto();
            if (string.IsNullOrEmpty(ID))
            {
                return BadRequest("ID không chính xác");
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var chungTu = _service.Repository.DbSet.FirstOrDefault(x => x.UNITCODE.Equals(unitCode) && x.ID.Equals(ID));
                if (chungTu != null)
                {
                    string connectString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
                    dto = Mapper.Map<CHUNGTU, Dto>(chungTu);
                    var chungTuChiTiet = _service.UnitOfWork.Repository<CHUNGTU_CHITIET>().DbSet.Where(x => x.MA_CHUNGTU.Equals(chungTu.MA_CHUNGTU)).ToList();
                    dto.DataDetails = Mapper.Map<List<CHUNGTU_CHITIET>, List<DtoDetails>>(chungTuChiTiet);
                    if(dto.DataDetails.Count > 0)
                    {
                        string listMatHang = "";
                        foreach(var matHang in dto.DataDetails)
                        {
                            listMatHang += matHang.MAHANG + ",";
                        }
                        listMatHang = listMatHang.Substring(0, listMatHang.Length - 1);
                        var MatHangViewModel = _service.GetDataMatHang(_service.ConvertConditionStringToArray(listMatHang), unitCode, connectString);
                        foreach (var row in dto.DataDetails)
                        {
                            var hang = MatHangViewModel.FirstOrDefault(x => x.MAHANG.Equals(row.MAHANG));
                            if(hang != null)
                            {
                                row.TENHANG = hang.TENHANG;
                                row.MADONVITINH = hang.MADONVITINH;
                                row.TYLE_LAILE = hang.TYLE_LAILE;
                            }
                        }
                    }
                }
                if (dto != null && !string.IsNullOrEmpty(dto.MA_CHUNGTU))
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

        [ResponseType(typeof(void))]
        [CustomAuthorize(Method = "SUA", State = "XuatBan")]
        public async Task<IHttpActionResult> Put (string id, Dto instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<CHUNGTU>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var item = _service.UpdateChungTu(instance);
                int upd = await _service.UnitOfWork.SaveAsync();
                if (upd > 1)
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


        [ResponseType(typeof(CHUNGTU))]
        [CustomAuthorize(Method = "XOA", State = "XuatBan")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            CHUNGTU instance = await _service.Repository.FindAsync(id);
            if (instance == null)
            {
                return NotFound();
            }
            try
            {
                var refund = _service.DeleteChungTu(instance.ID);
                int del = await _service.UnitOfWork.SaveAsync();
                if(del > 1)
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

        [Route("PostApproval")]
        [CustomAuthorize(Method = "DUYET", State = "XuatBan")]
        public async Task<IHttpActionResult> PostApproval(ParamApproval instance)
        {
            var result = new TransferObj();
            if (!string.IsNullOrEmpty(instance.ID))
            {
                string unitCode = _service.GetCurrentUnitCode();
                string connectString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
                CHUNGTU chungTu = _service.FindById(instance.ID);
                if (chungTu == null || chungTu.TRANGTHAI == (int)TypeState.APPROVAL)
                {
                    result.Status = false;
                    result.Data = "ISAPROVAL";
                    result.Message = "Không thể duyệt! Phiếu này đã được duyệt rồi";
                }
                else
                {
                    if (_service.Approval(chungTu.ID, connectString, chungTu, unitCode) != null)
                    {
                        //tính lại giá hàng hóa
                        int appro = await _service.UnitOfWork.SaveAsync();
                        if (appro > 0)
                        {
                            result.Data = chungTu;
                            result.Status = true;
                            result.Message = "Duyệt phiếu [" + chungTu.MA_CHUNGTU + "] thành công";
                        }
                        else
                        {
                            result.Data = null;
                            result.Status = false;
                            result.Message = "Duyệt không thành công";
                        }
                    }
                    else
                    {
                        result.Data = null;
                        result.Status = false;
                        result.Message = "Duyệt không thành công";
                    }
                }
            }
            else
            {
                result.Data = null;
                result.Status = false;
                result.Message = "ID không tìm thấy";
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