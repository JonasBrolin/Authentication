using System;
using System.Web.Http;

using Authentication.API.Providers;

using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

using Owin;

[assembly: OwinStartup(typeof(Authentication.API.Startup))]

namespace Authentication.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureOAuth(app);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions authAuthorizationServerOptions = new OAuthAuthorizationServerOptions 
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(authAuthorizationServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}