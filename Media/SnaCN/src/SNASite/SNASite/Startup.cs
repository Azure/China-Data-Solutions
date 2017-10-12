using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SNASite.Startup))]
namespace SNASite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
