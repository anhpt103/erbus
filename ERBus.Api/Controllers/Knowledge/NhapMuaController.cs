using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.KyKeToan;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.Knowledge.NhapMua;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Knowledge
{
    [RoutePrefix("api/Knowledge/NhapMua")]
    [Route("{id?}")]
    [Authorize]
    public class NhapMuaController : ApiController
    {
        private INhapMuaService _service;
        public NhapMuaController(INhapMuaService service)
        {
            _service = service;
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "NhapMua")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            TransferObj<PagedObj<CHUNGTU>> result = new TransferObj<PagedObj<CHUNGTU>>();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<NhapMuaViewModel.Search>>();
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


        public static CultureInfo GetVNeseCultureInfo()
        {
            int[] numArray = new int[] { 3 };
            int[] numArray1 = numArray;
            numArray = new int[1];
            int[] numArray2 = numArray;
            CultureInfo cultureInfo = new CultureInfo("vi-VN", true);
            cultureInfo.NumberFormat.CurrencyDecimalDigits = 0;
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.CurrencyGroupSeparator = ",";
            cultureInfo.NumberFormat.CurrencyGroupSizes = numArray1;
            cultureInfo.NumberFormat.CurrencySymbol = "";
            cultureInfo.NumberFormat.NumberDecimalDigits = 2;
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberGroupSeparator = ",";
            cultureInfo.NumberFormat.NumberGroupSizes = numArray1;
            cultureInfo.NumberFormat.PercentDecimalDigits = 1;
            cultureInfo.NumberFormat.PercentGroupSizes = numArray2;
            cultureInfo.NumberFormat.PercentDecimalSeparator = ".";
            cultureInfo.NumberFormat.PercentGroupSeparator = ",";
            cultureInfo.NumberFormat.PercentSymbol = "%";
            return cultureInfo;
        }
        public static string ConvertVNCurencyFormat(decimal number)
        {
            return number.ToString("C", GetVNeseCultureInfo());
        }
        public static string Formattienviet(string _string)
        {
            string str;
            try
            {
                str = ConvertVNCurencyFormat(decimal.Parse(_string));
            }
            catch
            {
                str = "0";
            }
            return str;
        }
        public String UnicodetoTCVN222(String strUnicode)
        {
            string strReturn = string.Empty;
            string strTest = "a,à,á,ả,ã,ạ,â,ầ,ấ,ẩ,ẫ,ậ,ă,ằ,ắ,ẳ,ẵ,ặ,e,è,é,ẻ,ẽ,ẹ,ê,ề,ế,ể,ễ,ệ,i,ì,í,ỉ,ĩ,ị,o,ò,ó,ỏ,õ,ọ,ơ,ờ,ớ,ở,ỡ,ợ,ô,ồ,ố,ổ,ỗ,ộ,u,ù,ú,ủ,ũ,ụ,ư,ừ,ứ,ử,ữ,ự,y,ỳ,ý,ỷ,ỹ,ỵ,đ";
            for (int j = 0; j < strUnicode.Length; j++)
            {
                if (strTest.Contains(strUnicode[j].ToString().ToLower()))
                {
                    //convert sang TCVN
                    StringBuilder strB = new StringBuilder(strUnicode[j].ToString().ToLower());
                    StringBuilder strtemp = new StringBuilder(strUnicode[j].ToString().ToLower());
                    #region chuyển mã kí tự unicode thường sang TCVN


                    //                            y          ỳ      ý       ỷ           ỹ       ỵ               
                    char[] Unicode_char = {             '\u1EF3','\u00FD','\u1EF7','\u1EF9','\u1EF5',
                //                            ư          ừ       ứ      ử           ữ      ự
                                            '\u01B0','\u1EEB','\u1EE9','\u1EED','\u1EEF','\u1EF1',
                //                            o          ò       ó      ỏ           õ      ọ
                                                     '\u00F2','\u00F3','\u1ECF','\u00F5','\u1ECD',
                //                            ơ          ờ       ớ      ở           ỡ      ợ
                                            '\u01A1','\u1EDD','\u1EDB','\u1EDF','\u1EE1','\u1EE3',
                //                            ô          ồ       ố      ổ           ỗ      ộ
                                            '\u00F4','\u1ED3','\u1ED1','\u1ED5','\u1ED7','\u1ED9',
                //                            i          ì       í      ỉ           ĩ      ị
                                                     '\u00EC','\u00ED','\u1EC9','\u0129','\u1ECB',
                //                            ê          ề       ế      ể           ễ      ệ
                                            '\u00EA','\u1EC1','\u1EBF','\u1EC3','\u1EC5','\u1EC7',
                //                            e          è       é      ẻ           ẽ      ẹ
                                                     '\u00E8','\u00E9','\u1EBB','\u1EBD','\u1EB9',
                //                            ă          ằ       ắ      ẳ           ẵ      ặ
                                            '\u0103','\u1EB1','\u1EAF','\u1EB3','\u1EB5','\u1EB7',
                //                            a          à       á      ả           ã      ạ
                                                     '\u00E0','\u00E1','\u1EA3','\u00E3','\u1EA1',
                //                            â          ầ       ấ      ẩ           ẫ      ậ
                                            '\u00E2','\u1EA7','\u1EA5','\u1EA9','\u1EAB','\u1EAD',
                //                            u          ù       ú      ủ           ũ      ụ
                                                     '\u00F9','\u00FA','\u1EE7','\u0169','\u1EE5',
                //                            đ
                                            '\u0111'};


                    //                            y          ỳ      ý       ỷ           ỹ       ỵ               
                    char[] TCVN_char = {                '\u00FA', '\u00FD','\u00FB','\u00FC','\u00FE',
                //                            ư          ừ       ứ      ử           ữ      ự
                                            '\u00AD','\u00F5','\u00F8','\u00F6','\u00F7','\u00F9',
                //                            o          ò       ó      ỏ           õ      ọ
                                                     '\u00DF','\u00E3','\u00E1','\u00E2','\u00E4',
                //                            ơ          ờ       ớ      ở           ỡ      ợ
                                            '\u00AC','\u00EA','\u00ED','\u00EB','\u00EC','\u00EE',
                //                            ô          ồ       ố      ổ           ỗ      ộ
                                            '\u00AB','\u00E5','\u00E8','\u00E6','\u00E7','\u00E9',
                //                            i          ì       í      ỉ           ĩ      ị
                                                     '\u00D7','\u00DD','\u00D8','\u00DC','\u00DE',
                //                            ê          ề       ế      ể           ễ      ệ
                                            '\u00AA','\u00D2','\u00D5','\u00D3','\u00D4','\u00D6',
                //                            e          è       é      ẻ           ẽ      ẹ
                                                     '\u00CC','\u00D0','\u00CE','\u00CF','\u00D1',
                //                            ă          ằ       ắ      ẳ           ẵ      ặ
                                            '\u00A8','\u00BB','\u00BE','\u00BC','\u00BD','\u00C6',
                //                            a          à       á      ả           ã      ạ
                                                     '\u00B5','\u00B8','\u00B6','\u00B7','\u00B9',
                //                            â          ầ       ấ      ẩ           ẫ      ậ
                                            '\u00A9','\u00C7','\u00CA','\u00C8','\u00C9','\u00CB',
                //                            u          ù       ú      ủ           ũ      ụ
                                                     '\u00EF','\u00F3','\u00F1','\u00F2','\u00F4',
                //                            đ
                                            '\u00AE'};

                    for (int i = 0; i < Unicode_char.Length; i++)
                    {
                        char a = Unicode_char[i];
                        char b = TCVN_char[i];
                        strB.Replace(a, b);
                        if (strtemp.ToString() != strB.ToString())
                        {
                            break;
                        }
                    }
                    strReturn = strReturn + strB.ToString();
                    #endregion
                }
                else
                {
                    strReturn = strReturn + strUnicode[j].ToString();
                }

            }
            return strReturn;
        }

        [Route("WriteDataToExcel")]
        public IHttpActionResult WriteDataToExcel(List<NhapMuaViewModel.PrintItemDto> listData)
        {
            var result = new TransferObj<List<NhapMuaViewModel.PrintItemDto>>();
            try
            {
                var filenameTemp = "TemplateNhapMua";
                var pathUpload = string.Format(@"~/Upload/BARCODE/");
                var pathTemplate = HostingEnvironment.MapPath(pathUpload);
                if (pathTemplate != null)
                {
                    var getDirectoryUpload = new DirectoryInfo(pathTemplate);
                    getDirectoryUpload.Create();
                }
                var tempFile = new FileInfo(pathTemplate + filenameTemp + ".xlsx");
                var pathRela = string.Format(@"~/Upload/BARCODE/");
                var pathAbs = HostingEnvironment.MapPath(pathRela);
                var getAbsoluteDirectoryInfo = new DirectoryInfo(pathAbs);
                getAbsoluteDirectoryInfo.Create();
                var filenameNew = @"Barcode.xls";
                FileInfo newFile = new FileInfo(pathAbs + filenameNew);
                if (newFile.Exists) 
                {
                    newFile.Delete();
                    newFile = new FileInfo(pathAbs + filenameNew);
                }
                using (ExcelPackage package = new ExcelPackage(newFile, tempFile))
                {
                    var worksheet = package.Workbook.Worksheets[1];
                    int index = 0;

                    for (int i = 0; i < listData.Count; i++)
                    {
                        for (int j = 0; j < listData[i].SOLUONG; j++)
                        {
                            worksheet.Cells[index + 2, 1].Value = index + 1;
                            worksheet.Cells[index + 2, 2].Value = listData[i].MAHANG;
                            worksheet.Cells[index + 2, 3].Value = UnicodetoTCVN222(listData[i].TENHANG);
                            worksheet.Cells[index + 2, 4].Value = listData[i].BARCODE;
                            worksheet.Cells[index + 2, 5].Value = Formattienviet(listData[i].GIABANLE_VAT.ToString()) + " VND";
                            worksheet.Cells[index + 2, 6].Value = Formattienviet(listData[i].GIABANLE_VAT.ToString()) + " VND";
                            worksheet.Cells[index + 2, 7].Value = listData[i].MANHACUNGCAP;
                            worksheet.Cells[index + 2, 8].Value = "1";
                            index++;
                        }
                    }
                    int totalRows = worksheet.Dimension.End.Row;
                    int totalCols = worksheet.Dimension.End.Column;
                    var dataCells = worksheet.Cells[2, 1, totalRows, totalCols];
                    var dataFont = dataCells.Style.Font;
                    dataFont.SetFromFont(new Font(".VnTime", 10));
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    HttpContext.Current.Response.Charset = "UTF-8";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    package.SaveAs(newFile);
                }
                result.Data = listData;
                result.Status = true;
                result.Message = filenameNew;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Data = null;
                result.Message = ex + "";
            }
            return Ok(result);
        }

        [Route("GetPrintItemDetails/{ID}")]
        public IHttpActionResult GetPrintItemDetails(string ID)
        {
            var result = new TransferObj<List<NhapMuaViewModel.PrintItemDto>>();
            List<NhapMuaViewModel.PrintItemDto> listData = _service.GetPrintItemDetails(ID, _service.GetConnectionString());
            if (listData.Count > 0)
            {
                result.Data = listData;
                result.Status = true;
            }
            else
            {
                result.Data = null;
                result.Status = false;
            }
            return Ok(result);
        }
        [ResponseType(typeof(CHUNGTU))]
        [CustomAuthorize(Method = "THEM", State = "NhapMua")]
        public async Task<IHttpActionResult> Post(NhapMuaViewModel.Dto instance)
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
            var result = new TransferObj<NhapMuaViewModel.Dto>();
            NhapMuaViewModel.Dto dto = new NhapMuaViewModel.Dto();
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
                    dto = Mapper.Map<CHUNGTU, NhapMuaViewModel.Dto>(chungTu);
                    var chungTuChiTiet = _service.UnitOfWork.Repository<CHUNGTU_CHITIET>().DbSet.Where(x => x.MA_CHUNGTU.Equals(chungTu.MA_CHUNGTU)).OrderByDescending(x => x.INDEX).ToList();
                    dto.DataDetails = Mapper.Map<List<CHUNGTU_CHITIET>, List<NhapMuaViewModel.DtoDetails>>(chungTuChiTiet);
                    if (dto.DataDetails.Count > 0)
                    {
                        string listMatHang = "";
                        foreach (var matHang in dto.DataDetails)
                        {
                            listMatHang += matHang.MAHANG + ",";
                        }
                        listMatHang = listMatHang.Substring(0, listMatHang.Length - 1);
                        var MatHangViewModel = _service.GetDataMatHang(_service.ConvertConditionStringToArray(listMatHang), unitCode, connectString);
                        foreach (var row in dto.DataDetails)
                        {
                            var hang = MatHangViewModel.FirstOrDefault(x => x.MAHANG.Equals(row.MAHANG));
                            if (hang != null)
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
        [CustomAuthorize(Method = "SUA", State = "NhapMua")]
        public async Task<IHttpActionResult> Put(string id, NhapMuaViewModel.Dto instance)
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
        [CustomAuthorize(Method = "XOA", State = "NhapMua")]
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
        [CustomAuthorize(Method = "DUYET", State = "NhapMua")]
        public async Task<IHttpActionResult> PostApproval(NhapMuaViewModel.ParamApproval instance)
        {
            var result = new TransferObj();
            string unitCode = _service.GetCurrentUnitCode();
            string connectString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
            //chưa khóa sổ
            List<KyKeToanViewModel.ViewModel> listChuaKhoa = _service.KyKeToanChuaKhoa(unitCode, connectString);
            if (listChuaKhoa != null && listChuaKhoa.Count > 0)
            {
                result.Status = false;
                result.Data = listChuaKhoa;
                result.Message = "HAVENOT_CLOSINGOUT_PERIOD";
            }
            else
            {
                if (!string.IsNullOrEmpty(instance.ID))
                {
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
                                if (_service.UpdateMatHangThayDoiGia(instance.ID, unitCode, connectString) > 0)
                                {
                                    result.Data = chungTu;
                                    result.Status = true;
                                    result.Message = "Duyệt phiếu [" + chungTu.MA_CHUNGTU + "] thành công ! Cập nhật giá";
                                }
                                else
                                {
                                    result.Data = chungTu;
                                    result.Status = true;
                                    result.Message = "Duyệt phiếu [" + chungTu.MA_CHUNGTU + "] thành công ! Lỗi cập nhật giá";
                                }

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