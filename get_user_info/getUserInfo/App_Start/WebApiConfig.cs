using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace getUserInfo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API の設定およびサービス

            // Web API ルート
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "EmployeeInfoApi",
            routeTemplate: "v1/jinji/empinfo",
            defaults: new { controller = "Jinji", action = "GetEmpInfo" }
            );

            config.Routes.MapHttpRoute(
            name: "BPInfoApi",
            routeTemplate: "v1/jinji/bpinfo",
            defaults: new { controller = "Jinji", action = "GetBPInfo" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
