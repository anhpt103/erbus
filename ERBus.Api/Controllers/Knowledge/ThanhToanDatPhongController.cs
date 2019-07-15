using AutoMapper;
using EASendMail;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity.Database.Catalog;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.Knowledge.DatPhong;
using ERBus.Service.Knowledge.ThanhToanDatPhong;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Knowledge
{
    [RoutePrefix("api/Knowledge/ThanhToanDatPhong")]
    [Route("{id?}")]
    [Authorize]
    public class ThanhToanDatPhongController : ApiController
    {
        private IThanhToanDatPhongService _service;
        public ThanhToanDatPhongController(IThanhToanDatPhongService service)
        {
            _service = service;
        }

        [Route("GetMerchandiseInBundleGoods")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "ThanhToanDatPhong")]
        public IHttpActionResult GetMerchandiseInBundleGoods(DatPhongViewModel.DatPhongPayDto data)
        {
            var result = new TransferObj<ThanhToanDatPhongViewModel.Dto>();
            result.Data = _service.GetMerchandiseInBundleGoods(_service.GetCurrentUnitCode(), _service.GetConnectionString(), data);
            if (result.Data != null)
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            return Ok(result);
        }

        [Route("GetHistoryPay")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "ThanhToanDatPhong")]
        public IHttpActionResult GetHistoryPay()
        {
            var result = new TransferObj<List<ThanhToanDatPhongViewModel.ViewModelHistory>>();
            result.Data = _service.GetHistory(_service.GetCurrentUnitCode(), _service.GetConnectionString());
            if (result.Data.Count > 0)
            {
                result.Status = true;
                result.Message = "Oke";
            }
            else
            {
                result.Status = false;
                result.Message = "NoData";
            }
            return Ok(result);
        }

        [Route("GetDetails/{ID}")]
        [HttpGet]
        public IHttpActionResult GetDetails(string ID)
        {
            var result = new TransferObj<ThanhToanDatPhongViewModel.Dto>();
            ThanhToanDatPhongViewModel.Dto dto = new ThanhToanDatPhongViewModel.Dto();
            if (string.IsNullOrEmpty(ID))
            {
                return BadRequest("ID không chính xác");
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var thanhToan = _service.Repository.DbSet.FirstOrDefault(x => x.UNITCODE.Equals(unitCode) && x.ID.Equals(ID));
                if (thanhToan != null)
                {
                    string connectString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
                    dto = Mapper.Map<THANHTOAN_DATPHONG, ThanhToanDatPhongViewModel.Dto>(thanhToan);
                    var nguoiPhucVu = _service.UnitOfWork.Repository<NGUOIDUNG>().DbSet.FirstOrDefault(x => x.USERNAME.Equals(dto.I_CREATE_BY) && x.UNITCODE.Equals(unitCode));
                    if (nguoiPhucVu != null) dto.PHUCVU = nguoiPhucVu.TENNHANVIEN;
                    var phong = _service.UnitOfWork.Repository<PHONG>().DbSet.FirstOrDefault(x => x.MAPHONG.Equals(dto.MAPHONG) && x.UNITCODE.Equals(unitCode));
                    if (phong != null)
                    {
                        dto.TENPHONG = phong.TENPHONG;
                        //lấy thông tin cấu hình phòng
                        var cauHinhPhong = _service.UnitOfWork.Repository<CAUHINH_LOAIPHONG>().DbSet.FirstOrDefault(x => x.MALOAIPHONG.Equals(phong.MALOAIPHONG) && x.UNITCODE.Equals(unitCode));
                        if(cauHinhPhong != null)
                        {
                            dto.MAHANG = cauHinhPhong.MAHANG;
                            dto.MAHANG_DICHVU = cauHinhPhong.MAHANG_DICHVU;
                        }
                    }
                    var thanhToanChiTiet = _service.UnitOfWork.Repository<THANHTOAN_DATPHONG_CHITIET>().DbSet.Where(x => x.MA_DATPHONG.Equals(thanhToan.MA_DATPHONG)).OrderByDescending(x => x.SAPXEP).ToList();
                    dto.DtoDetails = Mapper.Map<List<THANHTOAN_DATPHONG_CHITIET>, List<ThanhToanDatPhongViewModel.DtoDetail>>(thanhToanChiTiet);
                    if (dto.DtoDetails.Count > 0)
                    {
                        string listMatHang = "";
                        foreach (var matHang in dto.DtoDetails)
                        {
                            listMatHang += matHang.MAHANG + ",";
                        }
                        listMatHang = listMatHang.Substring(0, listMatHang.Length - 1);
                        var MatHangViewModel = _service.GetDataMatHang(_service.ConvertConditionStringToArray(listMatHang), unitCode, connectString);
                        foreach (var row in dto.DtoDetails)
                        {
                            var hang = MatHangViewModel.FirstOrDefault(x => x.MAHANG.Equals(row.MAHANG));
                            if (hang != null)
                            {
                                row.TENHANG = hang.TENHANG;
                            }
                            var donViTinh = _service.UnitOfWork.Repository<DONVITINH>().DbSet.FirstOrDefault(x => x.MADONVITINH.Equals(hang.MADONVITINH) && x.UNITCODE.Equals(unitCode));
                            if (donViTinh != null) row.DONVITINH = donViTinh.TENDONVITINH;
                            row.MATHUE_RA = hang.MATHUE_RA;
                            if (row.MAHANG.Equals(dto.MAHANG))
                            {
                                decimal TIENGIOHAT = 0;
                                int DONVI_THOIGIAN_TINHTIEN = 0;
                                int.TryParse(dto.DONVI_THOIGIAN_TINHTIEN.Value.ToString(), out DONVI_THOIGIAN_TINHTIEN);
                                if(DONVI_THOIGIAN_TINHTIEN != 0) decimal.TryParse(((row.GIABANLE_VAT / DONVI_THOIGIAN_TINHTIEN) * row.SOLUONG).ToString(), out TIENGIOHAT);
                                row.THANHTIEN = TIENGIOHAT;
                                dto.TIEN_GIOHAT = TIENGIOHAT;
                            } 
                            else row.THANHTIEN = row.SOLUONG * row.GIABANLE_VAT;
                        }
                    }
                }
                if (dto != null && !string.IsNullOrEmpty(dto.MA_DATPHONG))
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

        [Route("SenderGmail")]
        [ResponseType(typeof(THANHTOAN_DATPHONG))]
        [CustomAuthorize(Method = "THEM", State = "DatPhong")]
        public IHttpActionResult SenderGmail(ThanhToanDatPhongViewModel.ObjSendGmail obj)
        {
            var result = new TransferObj();
            var mailTask = new SmtpMail("TryIt");
            mailTask.Sender = "erbus.notification@gmail.com";
            mailTask.From = "erbus.notification@gmail.com";
            mailTask.To = "hathigiangnam1988@gmail.com";
            //mailTask.To = "phamtuananh10394@gmail.com";
            mailTask.Subject = "THANH TOÁN " + obj.MAPHONG + " (" + obj.MA_DATPHONG + ")";
            mailTask.HtmlBody = obj.BODY.ToString();
            try
            {
                var smtp = new SmtpClient();
                var server = new SmtpServer("smtp.gmail.com");
                server.Port = 587;
                server.Protocol = ServerProtocol.SMTP;
                server.ConnectType = SmtpConnectType.ConnectSSLAuto;
                server.User = "erbus.notification@gmail.com";
                server.Password = "1r0q5c3rewqASD!@#";
                smtp.SendMail(server, mailTask);
                result.Data = true;
                result.Message = "Send Gmail successfull!";
                result.Status = true;
            }
            catch
            {
                result.Data = false;
                result.Message = "Send Gmail error!";
                result.Status = false;
            }
            return Ok(result);
        }

        [Route("Post")]
        [ResponseType(typeof(THANHTOAN_DATPHONG))]
        [CustomAuthorize(Method = "THEM", State = "DatPhong")]
        public async Task<IHttpActionResult> Post(ThanhToanDatPhongViewModel.Dto instance)
        {
            var result = new TransferObj<THANHTOAN_DATPHONG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MA_DATPHONG == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MA_DATPHONG == instance.MA_DATPHONG && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã thanh toán đặt phòng này";
                    return Ok(result);
                }
            }
            try
            {
                var item = _service.InsertThanhToan(instance);
                int inst = await _service.UnitOfWork.SaveAsync();
                if (inst > 0)
                {
                    //TRỪ TỒN PHIẾU ĐẶT PHÒNG
                    _service.Approval(item.ID, _service.GetConnectionString(), curentUnitCode);
                    //Chuyển phiếu đặt phòng về bảng lịch sử và xóa phiếu đặt phòng vừa thanh toán
                    if (_service.UpdateTrangThaiDatPhong(item))
                    {
                        result.Status = true;
                        result.Data = item;
                        result.Message = "Thanh toán thành công";
                    }
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