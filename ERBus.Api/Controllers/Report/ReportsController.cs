using System.Web;
using Telerik.Reporting.Services.WebApi;

namespace ERBus.Api.Controllers.Report
{
    public class ReportsController : ReportsControllerBase
    {
        static Telerik.Reporting.Services.ReportServiceConfiguration configurationInstance =
            new Telerik.Reporting.Services.ReportServiceConfiguration
            {
                HostAppId = "ERBUS",
                ReportResolver = new ReportFileResolver(HttpContext.Current.Server.MapPath("~/Reports"))
                    .AddFallbackResolver(new ReportTypeResolver()),
                Storage = new Telerik.Reporting.Cache.File.FileStorage(),
            };
        public ReportsController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }
    }
}