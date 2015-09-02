using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IcedMemories.Startup))]
namespace IcedMemories
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
