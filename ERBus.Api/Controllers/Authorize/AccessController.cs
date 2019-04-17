using BTS.API.SERVICE.Authorize;
using ERBus.Service;
using System.Web.Http;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/Authorize/Access")]
    [Route("{id?}")]
    [Authorize]
    public class AccessController : ApiController
    {
        private readonly IAccessService _service;
        public AccessController(IAccessService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("GetAccesslist/{machucnang}")]
        public RoleState GetAccesslist(string machucnang)
        {
            var _unitCode = _service.GetCurrentUnitCode();
            RoleState roleState = _service.GetRoleStateByMaChucNang(_unitCode, RequestContext.Principal.Identity.Name, machucnang);
            return roleState;
        }
    }
}