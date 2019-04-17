using ERBus.Api.App_Start;
using ERBus.Service;
using System;
using System.Threading;
using System.Web.Http;
using Telerik.Reporting.Services.WebApi;

namespace ERBus.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Telerik
            ReportsControllerConfiguration.RegisterRoutes(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.Config();
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Set("Server", "ERBusServer");
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is ThreadAbortException) return;
        }
    }
}