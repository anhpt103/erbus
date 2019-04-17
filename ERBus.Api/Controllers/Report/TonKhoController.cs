using ERBus.Service.Authorize.KyKeToan;
using System.Web.Http;

namespace ERBus.Api.Controllers.Report
{
    [RoutePrefix("api/Report/TonKho")]
    [Route("{id?}")]
    [Authorize]
    public class XuatNhapTonController : ApiController
    {
        private IKyKeToanService _service;
        public XuatNhapTonController(IKyKeToanService service)
        {
            _service = service;
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