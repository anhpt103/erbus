using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.Promotion.GiamGiaNhomHang;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Promotion
{
    [RoutePrefix("api/Promotion/GiamGiaNhomHang")]
    [Route("{id?}")]
    [Authorize]
    public class GiamGiaNhomHangController : ApiController
    {
        private IGiamGiaNhomHangService _service;
        public GiamGiaNhomHangController(IGiamGiaNhomHangService service)
        {
            _service = service;
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "GiamGiaNhomHang")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<KHUYENMAI>> result = new TransferObj<PagedObj<KHUYENMAI>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<GiamGiaNhomHangViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<KHUYENMAI>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                PagedObj<KHUYENMAI> tempData = _service.QueryPageGiamGiaNhomHang(_service.GetConnectionString(), paged, filtered.Summary, unitCode);
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

        [ResponseType(typeof(KHUYENMAI))]
        [CustomAuthorize(Method = "THEM", State = "GiamGiaNhomHang")]
        public async Task<IHttpActionResult> Post(GiamGiaNhomHangViewModel.Dto instance)
        {
            var result = new TransferObj<KHUYENMAI>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MA_KHUYENMAI == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MA_KHUYENMAI == instance.MA_KHUYENMAI && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại chứng từ này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MA_KHUYENMAI = _service.SaveCode();
                var item = _service.InsertKhuyenMai(instance);
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
            var result = new TransferObj<GiamGiaNhomHangViewModel.Dto>();
            GiamGiaNhomHangViewModel.Dto dto = new GiamGiaNhomHangViewModel.Dto();
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
                    dto = Mapper.Map<KHUYENMAI, GiamGiaNhomHangViewModel.Dto>(chungTu);
                    List<GiamGiaNhomHangViewModel.DtoDetails> listDetails = new List<GiamGiaNhomHangViewModel.DtoDetails>();
                    var chungTuChiTiet = _service.UnitOfWork.Repository<KHUYENMAI_CHITIET>().DbSet.Where(x => x.MA_KHUYENMAI.Equals(chungTu.MA_KHUYENMAI)).OrderByDescending(x => x.INDEX).ToList();
                    if (chungTuChiTiet.Count > 0)
                    {

                        foreach (var row in chungTuChiTiet)
                        {
                            GiamGiaNhomHangViewModel.DtoDetails details = new GiamGiaNhomHangViewModel.DtoDetails();
                            details.ID = row.ID;
                            details.MANHOM = row.MAHANG;
                            details.SOLUONG = row.SOLUONG;
                            var nhomHang = _service.UnitOfWork.Repository<NHOMHANG>().DbSet.FirstOrDefault(x => x.MANHOM.Equals(row.MAHANG));
                            details.TENNHOM = nhomHang != null ? nhomHang.TENNHOM : "";
                            details.GIATRI_KHUYENMAI = row.GIATRI_KHUYENMAI;
                            details.INDEX = row.INDEX;
                            listDetails.Add(details);
                        }
                    }
                    dto.DataDetails = listDetails;
                }
                if (dto != null && !string.IsNullOrEmpty(dto.MA_KHUYENMAI))
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
        [CustomAuthorize(Method = "SUA", State = "GiamGiaNhomHang")]
        public async Task<IHttpActionResult> Put(string id, GiamGiaNhomHangViewModel.Dto instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<KHUYENMAI>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var item = _service.UpdateKhuyenMai(instance);
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


        [ResponseType(typeof(KHUYENMAI))]
        [CustomAuthorize(Method = "XOA", State = "GiamGiaNhomHang")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            KHUYENMAI instance = await _service.Repository.FindAsync(id);
            if (instance == null)
            {
                return NotFound();
            }
            try
            {
                var refund = _service.DeleteKhuyenMai(instance.ID);
                int del = await _service.UnitOfWork.SaveAsync();
                if (del > 1)
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
        [CustomAuthorize(Method = "DUYET", State = "GiamGiaNhomHang")]
        public async Task<IHttpActionResult> PostApproval(GiamGiaNhomHangViewModel.ParamApproval instance)
        {
            var result = new TransferObj();
            string unitCode = _service.GetCurrentUnitCode();
            if (!string.IsNullOrEmpty(instance.ID))
            {
                KHUYENMAI chungTu = _service.FindById(instance.ID);
                if (chungTu == null || chungTu.TRANGTHAI == (int)TypeState.APPROVAL)
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "ISAPROVAL" ;
                }
                else
                {
                    chungTu.TRANGTHAI = (int)TypeState.APPROVAL;
                    chungTu.THOIGIAN_DUYET = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    chungTu.ObjectState = ObjectState.Modified;
                    _service.Update(chungTu);
                    int appro = await _service.UnitOfWork.SaveAsync();
                    if (appro > 0)
                    {
                        result.Data = chungTu;
                        result.Status = true;
                        result.Message = "Kích hoạt [" + chungTu.MA_KHUYENMAI + "] thành công !";

                    }
                    else
                    {
                        result.Data = null;
                        result.Status = false;
                        result.Message = "Kích hoạt không thành công";
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

        [Route("PostUnApproval")]
        [CustomAuthorize(Method = "DUYET", State = "GiamGiaNhomHang")]
        public async Task<IHttpActionResult> PostUnApproval(GiamGiaNhomHangViewModel.ParamApproval instance)
        {
            var result = new TransferObj();
            string unitCode = _service.GetCurrentUnitCode();
            if (!string.IsNullOrEmpty(instance.ID))
            {
                KHUYENMAI chungTu = _service.FindById(instance.ID);
                if (chungTu == null || chungTu.TRANGTHAI == (int)TypeState.NOTAPPROVAL)
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "ISUNAPROVAL";
                }
                else
                {
                    chungTu.TRANGTHAI = (int)TypeState.NOTAPPROVAL;
                    chungTu.THOIGIAN_DUYET = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    chungTu.ObjectState = ObjectState.Modified;
                    _service.Update(chungTu);
                    int appro = await _service.UnitOfWork.SaveAsync();
                    if (appro > 0)
                    {
                        result.Data = chungTu;
                        result.Status = true;
                        result.Message = "Bỏ kích hoạt [" + chungTu.MA_KHUYENMAI + "] thành công !";

                    }
                    else
                    {
                        result.Data = null;
                        result.Status = false;
                        result.Message = "Bỏ kích hoạt không thành công";
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