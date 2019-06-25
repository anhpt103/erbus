using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Catalog.MatHang;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/MatHang")]
    [Route("{id?}")]
    [Authorize]
    public class MatHangController : ApiController
    {
        private IMatHangService _service;
        public MatHangController(IMatHangService service)
        {
            _service = service;
        }

        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "MatHang")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode)).OrderBy(x => x.MALOAI).Select(x => new ChoiceObject { VALUE = x.MAHANG, TEXT = x.MAHANG + "|" + x.TENHANG, DESCRIPTION = x.TENHANG, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
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


        [Route("GetAllMatHang/{maHang}")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "MatHang")]
        public IHttpActionResult GetAllMatHang(string maHang)
        {
            var result = new TransferObj<List<MATHANG>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode) && x.MAHANG.Equals(maHang.ToUpper())).OrderBy(x => x.MALOAI).ToList();
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
        

        [Route("GetAllBarcode")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "MatHang")]
        public IHttpActionResult GetAllBarcode()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode) && x.BARCODE != null).OrderBy(x => x.MAHANG).Select(x => new ChoiceObject { VALUE = x.MAHANG, TEXT = x.BARCODE}).ToList();
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
        [CustomAuthorize(Method = "XEM", State = "MatHang")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<MatHangViewModel.VIEW_MODEL>> result = new TransferObj<PagedObj<MatHangViewModel.VIEW_MODEL>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<MatHangViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<MatHangViewModel.VIEW_MODEL>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                PagedObj<MatHangViewModel.VIEW_MODEL> tempData = _service.QueryPageMatHang(_service.GetConnectionString(), paged, filtered.Summary, unitCode);
                result.Data = tempData;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("PostQueryInventory")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "MatHang")]
        public IHttpActionResult PostQueryInventory(JObject jsonData)
        {
            TransferObj<PagedObj<MatHangViewModel.VIEW_MODEL>> result = new TransferObj<PagedObj<MatHangViewModel.VIEW_MODEL>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<MatHangViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<MatHangViewModel.VIEW_MODEL>>();
            var unitCode = _service.GetCurrentUnitCode();
            try
            {
                string TableName = filtered.AdvanceData.TABLE_NAME;
                string MaKho = filtered.AdvanceData.MAKHO;
                PagedObj<MatHangViewModel.VIEW_MODEL> tempData = _service.QueryPageMatHangInventory(_service.GetConnectionString(), paged, filtered.Summary, unitCode, TableName, MaKho);
                result.Data = tempData;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("BuildNewCode/{maLoaiSelected}")]
        [HttpGet]
        public string BuildNewCode(string maLoaiSelected)
        {
            return _service.BuildCode(maLoaiSelected);
        }

        [Route("GetDetails/{ID}")]
        [HttpGet]
        public IHttpActionResult GetDetails(string ID)
        {
            var result = new TransferObj<MatHangViewModel.Dto>();
            MatHangViewModel.Dto dto = new MatHangViewModel.Dto();
            if (string.IsNullOrEmpty(ID))
            {
                return BadRequest("ID không chính xác");
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var matHang = _service.Repository.DbSet.FirstOrDefault(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode) && x.ID.Equals(ID));
                if (matHang != null)
                {
                    dto = Mapper.Map<MATHANG, MatHangViewModel.Dto>(matHang);
                    dto.DUONGDAN = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/" + dto.DUONGDAN;
                    var matHangGia = _service.UnitOfWork.Repository<MATHANG_GIA>().DbSet.FirstOrDefault(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode) && x.MAHANG.Equals(matHang.MAHANG));
                    if (matHangGia != null)
                    {
                        dto.GIAMUA = matHangGia.GIAMUA;
                        dto.GIAMUA_VAT = matHangGia.GIAMUA_VAT;
                        dto.GIABANLE = matHangGia.GIABANLE;
                        dto.GIABANLE_VAT = matHangGia.GIABANLE_VAT;
                        dto.GIABANBUON = matHangGia.GIABANBUON;
                        dto.GIABANBUON_VAT = matHangGia.GIABANBUON_VAT;
                        dto.TYLE_LAILE = matHangGia.TYLE_LAILE;
                        dto.TYLE_LAIBUON = matHangGia.TYLE_LAIBUON;
                    }
                }
                if (dto != null && !string.IsNullOrEmpty(dto.MAHANG))
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


        [Route("GetMatHangNhapMuaTheoMaKho")]
        [HttpPost]
        public IHttpActionResult GetMatHangNhapMuaTheoMaKho(MatHangViewModel.PARAM_NHAPMUA_OBJ param)
        {
            var result = new TransferObj<MatHangViewModel.VIEW_MODEL>();
            var viewModel = new MatHangViewModel.VIEW_MODEL();
            if (string.IsNullOrEmpty(param.MAHANG))
            {
                return BadRequest("Mã hàng không chính xác");
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var listSearched = _service.TimKiemMatHang_NhieuDieuKien(param.MAHANG, unitCode, _service.GetConnectionString());
                if (listSearched != null && listSearched.Count == 1)
                {
                    result.Data = listSearched[0];
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


        [Route("GetMatHangXuatBanTheoMaKho")]
        [HttpPost]
        public IHttpActionResult GetMatHangXuatBanTheoMaKho(MatHangViewModel.PARAM_NHAPMUA_OBJ param)
        {
            var result = new TransferObj<MatHangViewModel.VIEW_MODEL>();
            var viewModel = new MatHangViewModel.VIEW_MODEL();
            if (string.IsNullOrEmpty(param.MAHANG))
            {
                result.Data = null;
                result.Message = "NOTEXISTS_MAHANG";
                result.Status = false;
            }
            else if (string.IsNullOrEmpty(param.MAKHO_XUAT))
            {
                result.Data = null;
                result.Message = "NOTEXISTS_MAKHO_XUAT";
                result.Status = false;
            }
            else if (string.IsNullOrEmpty(param.TABLE_NAME))
            {
                result.Data = null;
                result.Message = "NOTEXISTS_TABLE_NAME";
                result.Status = false;
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var listSearched = _service.TimKiemMatHang_TonKho_NhieuDieuKien(param.MAHANG, unitCode, _service.GetConnectionString(), param.TABLE_NAME, param.MAKHO_XUAT);
                if (listSearched != null && listSearched.Count == 1)
                {
                    result.Data = listSearched[0];
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


        [Route("GetMatHangTheoDieuKien")]
        [HttpPost]
        public IHttpActionResult GetMatHangTheoDieuKien(MatHangViewModel.PARAM_NHAPMUA_OBJ param)
        {
            var result = new TransferObj<MatHangViewModel.VIEW_MODEL>();
            var viewModel = new MatHangViewModel.VIEW_MODEL();
            if (string.IsNullOrEmpty(param.MAHANG))
            {
                return BadRequest("Mã hàng không chính xác");
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var listSearched = _service.TimKiemMatHang_NhieuDieuKien(param.MAHANG, unitCode, _service.GetConnectionString());
                if (listSearched != null && listSearched.Count == 1)
                {
                    result.Data = listSearched[0];
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

        [Route("SearchDataByKey")]
        [HttpPost]
        public IHttpActionResult SearchDataByKey(MatHangViewModel.PARAM_SEARCH_OBJ param)
        {
            var result = new TransferObj<List<MatHangViewModel.VIEW_MODEL>>();
            var unitCode = _service.GetCurrentUnitCode();

            if (string.IsNullOrEmpty(param.KEYSEARCH))
            {
                return BadRequest("Thiếu điều kiện đầu vào");
            }
            else
            {
                var listSearched = _service.TimKiemMatHang_NhieuDieuKien(param.KEYSEARCH,unitCode, _service.GetConnectionString());
                if (listSearched != null && listSearched.Count > 0)
                {
                    result.Data = listSearched;
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

        [Route("CheckExistBarcode/{barcodeText}")]
        [HttpGet]
        public IHttpActionResult CheckExistBarcode(string barcodeText)
        {
            var result = new TransferObj<string>();
            try
            {
                string refund = _service.FindExistsBarCode(barcodeText, _service.GetConnectionString());
                if (!string.IsNullOrEmpty(refund))
                {
                    result.Data = refund;
                    result.Message = "Đã tồn tại mã BARCODE ở mặt hàng [" + refund + "]";
                    result.Status = false;
                }
                else
                {
                    result.Data = null;
                    result.Message = "NotExists";
                    result.Status = true;
                }
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Message = "Exception:" + e.ToString();
                result.Status = true;
            }
            return Ok(result);
        }

        [Route("UploadImage")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult UploadImage()
        {
            var result = new TransferObj<MatHangViewModel.InfoUpload>();
            try
            {
                MatHangViewModel.InfoUpload data = _service.UploadImage(false);
                if (data != null && !string.IsNullOrEmpty(data.FILENAME))
                {
                    result.Status = true;
                    result.Data = data;
                    result.Message = "Success";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Not Success";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Data = null;
                result.Message = "Exception:" + e.ToString();
            }
            return Ok(result);
        }

        [Route("UploadAvatar")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult UploadAvatar()
        {
            var result = new TransferObj<MatHangViewModel.InfoUpload>();
            try
            {
                MatHangViewModel.InfoUpload data = _service.UploadImage(true);
                if (data != null && !string.IsNullOrEmpty(data.FILENAME))
                {
                    result.Status = true;
                    result.Data = data;
                    result.Message = "Success";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Not Success";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Data = null;
                result.Message = "Exception:" + e.ToString();
            }
            return Ok(result);
        }

        [Route("Post")]
        [ResponseType(typeof(MATHANG))]
        [CustomAuthorize(Method = "THEM", State = "MatHang")]
        public async Task<IHttpActionResult> Post(MatHangViewModel.Dto instance)
        {
            var result = new TransferObj<MATHANG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MAHANG == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MAHANG == instance.MAHANG && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã mặt hàng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MAHANG = _service.SaveCode(instance.MALOAI);
                var item = _service.InsertDto(instance);
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
        [CustomAuthorize(Method = "SUA", State = "MatHang")]
        public async Task<IHttpActionResult> Put(string id, MatHangViewModel.Dto instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<MATHANG>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var item = _service.UpdateDto(instance);
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


        [ResponseType(typeof(MATHANG))]
        [CustomAuthorize(Method = "XOA", State = "MatHang")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            MATHANG instance = await _service.Repository.FindAsync(id);
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

        [Route("PostExcelData")]
        [ResponseType(typeof(bool))]
        [CustomAuthorize(Method = "THEM", State = "MatHang")]
        public IHttpActionResult PostExcelData(List<MatHangViewModel.Dto> listExcelData)
        {
            var result = new TransferObj<bool>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (listExcelData.Count > 0)
            {
                try
                {
                    if(_service.InsertDataExcel(listExcelData, curentUnitCode, _service.GetConnectionString()))
                    {
                        result.Status = true;
                        result.Data = true;
                        result.Message = "Nhận dữ liệu hàng hóa thành công";
                    }
                    else
                    {
                        result.Status = false;
                        result.Data = false;
                        result.Message = "Thao tác không thành công";
                    }
                }
                catch (Exception e)
                {
                    result.Status = false;
                    result.Message = e.Message;
                }
            }
            return Ok(result);
        }

        [Route("GetPhysicalPathTemplate")]
        [HttpGet]
        public string GetPhysicalPathTemplate()
        {
            return _service.PhysicalPathTemplate();
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