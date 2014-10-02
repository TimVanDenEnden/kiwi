﻿using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using Lisa.Kiwi.Data.Models;

namespace Lisa.Kiwi.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "KiwiReportApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Web API configuration and services
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<Models.Report>("Report");
            builder.EntitySet<Contact>("Contact");
            builder.EntitySet<Remark>("Remark");
            builder.EntitySet<Status>("Status");

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            //ODataMediaTypeFormatter odataFormatter = new ODataMediaTypeFormatter();
            //odataFormatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
        }
    }
}
