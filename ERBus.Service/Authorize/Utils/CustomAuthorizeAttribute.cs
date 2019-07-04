using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ERBus.Service.Authorize.Utils
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string State { get; set; }
        public string Method { get; set; }
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string _unitCode = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.User is ClaimsPrincipal)
            {
                var currentUser = (HttpContext.Current.User as ClaimsPrincipal);
                var unit = currentUser.Claims.FirstOrDefault(x => x.Type == "unitCode");
                var parentUnitCode = currentUser.Claims.FirstOrDefault(x => x.Type == "parentUnitCode");
                _unitCode = string.IsNullOrEmpty(parentUnitCode != null ? parentUnitCode.Value : "") ? (unit != null ? unit.Value : "") : (parentUnitCode != null ? parentUnitCode.Value : "");
            }
            string username = actionContext.RequestContext.Principal.Identity.Name;
            var authorize = base.IsAuthorized(actionContext);
            if ((username.Equals("admin")) && authorize) return true;
            bool check = false;
            RoleState roleState = Get(_unitCode, username, State);
            if (!string.IsNullOrEmpty(roleState?.STATE))
            {
                switch (Method)
                {
                    case "XEM":
                        if (roleState.XEM) check = true;
                        break;
                    case "THEM":
                        if (roleState.THEM) check = true;
                        break;
                    case "SUA":
                        if (roleState.SUA) check = true;
                        break;
                    case "XOA":
                        if (roleState.XOA) check = true;
                        break;
                    case "DUYET":
                        if (roleState.DUYET) check = true;
                        break;
                }
            }
            if (!authorize || !check) return false;
            return true;

        }

        private RoleState Get(string unitCode, string username, string machucnang)
        {
            RoleState roleState = new RoleState();
            if (username.Equals("admin"))
            {
                roleState = new RoleState()
                {
                    XEM = true,
                    THEM = true,
                    SUA = true,
                    XOA = true,
                    DUYET = true,
                    STATE = "all",
                };
            }
            else
            {
                var cacheData = MemoryCacheHelper.GetValue(unitCode + "|" + machucnang + "|" + username);
                if (cacheData == null)
                {
                    using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                    {
                        connection.Open();
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                @"SELECT XEM,THEM,SUA,XOA,DUYET FROM NHOMQUYEN_MENU WHERE UNITCODE LIKE '" + unitCode + "%' AND MA_MENU='" + machucnang +
                                "' AND MANHOMQUYEN IN (SELECT MANHOMQUYEN FROM NGUOIDUNG_NHOMQUYEN WHERE UNITCODE LIKE '" + unitCode + "%' AND USERNAME='" +
                                username + "') UNION SELECT NGUOIDUNG_MENU.XEM,NGUOIDUNG_MENU.THEM,NGUOIDUNG_MENU.SUA,NGUOIDUNG_MENU.XOA,NGUOIDUNG_MENU.DUYET " +
                                "FROM NGUOIDUNG_MENU WHERE NGUOIDUNG_MENU.UNITCODE LIKE '" + unitCode + "%' AND NGUOIDUNG_MENU.MA_MENU='" + machucnang + "' AND NGUOIDUNG_MENU.USERNAME='" + username + "'";
                            using (OracleDataReader oracleDataReader = command.ExecuteReader())
                            {
                                if (!oracleDataReader.HasRows)
                                {
                                    roleState = new RoleState()
                                    {
                                        STATE = string.Empty,
                                        XEM = false,
                                        THEM = false,
                                        SUA = false,
                                        XOA = false,
                                        DUYET = false,
                                    };
                                }
                                else
                                {
                                    roleState.STATE = machucnang;
                                    while (oracleDataReader.Read())
                                    {
                                        int objXem = Int32.Parse(oracleDataReader["XEM"].ToString());
                                        if (objXem == 1)
                                        {
                                            roleState.XEM = true;
                                        }
                                        int objThem = Int32.Parse(oracleDataReader["THEM"].ToString());
                                        if (objThem == 1)
                                        {
                                            roleState.THEM = true;
                                        }
                                        int objSua = Int32.Parse(oracleDataReader["SUA"].ToString());
                                        if (objSua == 1)
                                        {
                                            roleState.SUA = true;
                                        }
                                        int objXoa = Int32.Parse(oracleDataReader["XOA"].ToString());
                                        if (objXoa == 1)
                                        {
                                            roleState.XOA = true;
                                        }
                                        int objDuyet = Int32.Parse(oracleDataReader["DUYET"].ToString());
                                        if (objDuyet == 1)
                                        {
                                            roleState.DUYET = true;
                                        }
                                    }
                                    MemoryCacheHelper.Add(unitCode + "|" + machucnang + "|" + username, roleState,
                                        DateTimeOffset.Now.AddHours(6));
                                }
                            }
                        }
                    }
                }
                else
                {
                    roleState = (RoleState)cacheData;
                }
            }
            return roleState;
        }
    }
}
