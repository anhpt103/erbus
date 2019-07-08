using EASendMail;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.Knowledge.DatPhong;
using ERBus.Service.Knowledge.ThanhToanDatPhong;
using System;
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

        [Route("SenderGmail")]
        [ResponseType(typeof(THANHTOAN_DATPHONG))]
        [CustomAuthorize(Method = "THEM", State = "DatPhong")]
        public IHttpActionResult SenderGmail(ThanhToanDatPhongViewModel.ObjSendGmail obj)
        {
            var result = new TransferObj();
            var mailTask = new SmtpMail("TryIt");
            mailTask.Sender = "erbus.notification@gmail.com";
            mailTask.From = "erbus.notification@gmail.com";
            mailTask.To = "thuynguyentd11@gmail.com";
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