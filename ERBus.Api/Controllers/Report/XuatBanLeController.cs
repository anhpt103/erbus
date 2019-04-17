using ERBus.Service.Authorize.KyKeToan;
using System.Web.Http;

namespace ERBus.Api.Controllers.Report
{
    [RoutePrefix("api/Report/XuatBanLe")]
    [Route("{id?}")]
    [Authorize]
    public class XuatBanLeController : ApiController
    {
        private IKyKeToanService _service;
        public XuatBanLeController(IKyKeToanService service)
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