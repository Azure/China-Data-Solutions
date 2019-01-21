using System.Web.Hosting;
using System.Web.Http;

namespace Iotc.Web.Backend
{
    public static class WebApiConfig
    {
        public static string DataPath = "";
    
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            DataPath = HostingEnvironment.MapPath("~/Data/");

    }
}
}
