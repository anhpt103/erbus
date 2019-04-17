using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERBus.Internet.Startup))]
namespace ERBus.Internet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
