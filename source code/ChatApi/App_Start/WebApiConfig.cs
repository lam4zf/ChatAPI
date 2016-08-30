using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;


namespace ChatAPI
{
    public static class WebApiConfig
    {
		public static HttpConfiguration Register()
		{
			var config = new HttpConfiguration();

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(name: "DefaultRouting",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional });

			config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
			config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			return config;
		}

        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
