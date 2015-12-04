using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISSolutions.Startup))]
namespace ISSolutions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
