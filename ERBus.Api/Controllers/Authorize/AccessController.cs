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
        [Route("GetAccesslist/{machucnang}/{userName}/{unitCode}")]
        public RoleState GetAccesslist(string machucnang, string userName, string unitCode)
        {
            RoleState roleState = _service.GetRoleStateByMaChucNang(unitCode, userName, machucnang);
            return roleState;
        }

        [HttpPost]
        [Route("ClosingOutMultiplePeriod")]
        public bool ClosingOutMultiplePeriod()
        {
            bool closing = _service.ClosingOutMultiple();
            return closing;
        }
        
    }
}