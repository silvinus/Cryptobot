using Cryptobot.CoinMarketCap;
using Cryptobot.Interface;
using CryptobotFull.Resolver;
using CryptobotFull.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace CryptobotFull
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // IoC container
            var container = new UnityContainer();
            container.RegisterType<IMarket, CoinMarketClients>(new HierarchicalLifetimeManager());
            container.RegisterType<ICryptoStorage, CryptoStorage>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Json settings
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            // Web API configuration and services

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
