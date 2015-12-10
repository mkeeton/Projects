using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISSolutions.Home.Startup))]
namespace ISSolutions.Home
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
