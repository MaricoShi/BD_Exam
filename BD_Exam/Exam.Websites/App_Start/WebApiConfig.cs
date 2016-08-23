using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using System.Web.Http.OData;
using System.Web.Http.OData.Builder;

namespace Exam.Websites
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapODataRoute("odata", "odata", ODataConfig.GetEdmModel());
            config.EnableQuerySupport();
        }
    }
}
