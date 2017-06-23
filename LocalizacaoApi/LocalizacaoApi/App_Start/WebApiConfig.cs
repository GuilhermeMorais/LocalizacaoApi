using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Configuration;
using System.Web.Http;
#pragma warning disable 1591

namespace LocalizacaoApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Json
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                ////JsonP (call from sites)
                //var formatter = new JsonpMediaTypeFormatter(jsonFormatter);
                //config.Formatters.Insert(0, formatter);
            }

            var usaHttps = WebConfigurationManager.AppSettings["usaHttps"];
            if (usaHttps.Equals("true"))
            {
                config.Filters.Add(new Filters.RequireHttpsAttribute());
            }
        }
    }
}
