﻿using System;
using System.Data.Entity;
using System.Web.Http;
using Lisa.Kiwi.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.AspNet.SignalR;
using System.Data.Entity.Migrations;

[assembly: OwinStartup(typeof(Lisa.Kiwi.WebApi.Startup))]

namespace Lisa.Kiwi.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var signalRConfig = new HubConfiguration()
            {
                EnableJSONP = true
            };
            app.MapSignalR("/signalr/signalr", signalRConfig);

            var config = new HttpConfiguration();

            ConfigureOAuth(app);

            // Set up Owin to use the WebAPI's config
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private static void ConfigureOAuth(IAppBuilder app)
        {
            var serverOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/oauth"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(serverOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}