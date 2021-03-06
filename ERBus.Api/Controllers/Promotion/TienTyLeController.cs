﻿using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.Promotion.TienTyLe;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Promotion
{
    [RoutePrefix("api/Promotion/TienTyLe")]
    [Route("{id?}")]
    [Authorize]
    public class TienTyLeController : ApiController
    {
        private ITienTyLeService _service;
        public TienTyLeController(ITienTyLeService service)
        {
            _service = service;
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "TienTyLe")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<KHUYENMAI>> result = new TransferObj<PagedObj<KHUYENMAI>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<TienTyLeViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<KHUYENMAI>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                PagedObj<KHUYENMAI> tempData = _service.QueryPageTienTyLe(_service.GetConnectionString(), paged, filtered.Summary, unitCode);
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
        [CustomAuthorize(Method = "THEM", State = "TienTyLe")]
        public async Task<IHttpActionResult> Post(TienTyLeViewModel.Dto instance)
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
            var result = new TransferObj<TienTyLeViewModel.Dto>();
            TienTyLeViewModel.Dto dto = new TienTyLeViewModel.Dto();
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
                    dto = Mapper.Map<KHUYENMAI, TienTyLeViewModel.Dto>(chungTu);
                    var chungTuChiTiet = _service.UnitOfWork.Repository<KHUYENMAI_CHITIET>().DbSet.Where(x => x.MA_KHUYENMAI.Equals(chungTu.MA_KHUYENMAI)).OrderByDescending(x => x.INDEX).ToList();
                    dto.DataDetails = Mapper.Map<List<KHUYENMAI_CHITIET>, List<TienTyLeViewModel.DtoDetails>>(chungTuChiTiet);
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
                                row.GIABANLE_VAT = hang.GIABANLE_VAT;
                            }
                        }
                    }
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
        [CustomAuthorize(Method = "SUA", State = "TienTyLe")]
        public async Task<IHttpActionResult> Put(string id, TienTyLeViewModel.Dto instance)
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
        [CustomAuthorize(Method = "XOA", State = "TienTyLe")]
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
        [CustomAuthorize(Method = "DUYET", State = "TienTyLe")]
        public async Task<IHttpActionResult> PostApproval(TienTyLeViewModel.ParamApproval instance)
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
        [CustomAuthorize(Method = "DUYET", State = "TienTyLe")]
        public async Task<IHttpActionResult> PostUnApproval(TienTyLeViewModel.ParamApproval instance)
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