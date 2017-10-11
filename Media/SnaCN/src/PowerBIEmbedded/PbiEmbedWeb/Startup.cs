using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PbiEmbedWeb.Startup))]
namespace PbiEmbedWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
