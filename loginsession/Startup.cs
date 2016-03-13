using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(loginsession.Startup))]
namespace loginsession
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
