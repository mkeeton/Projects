using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using IcedMemories.Data.Interfaces;
using IcedMemories.Infrastructure.Interfaces;
using IcedMemories.Infrastructure.Repositories;
using IcedMemories.Domain.Models;
using IcedMemories.Infrastructure;
using System.Web;
using System.Web.Configuration;

namespace IcedMemories
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
          // Configure the db context and user manager to use a single instance per request
          //WebConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString
          app.CreatePerOwinContext<IDbContext>(() => new IcedMemories.Data.DbContext.DapperAccessDbContext(WebConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString));
          app.CreatePerOwinContext<IUnitOfWork>(() => UnitOfWorkOle.Create(HttpContext.Current.GetOwinContext().Get<IDbContext>()));
          app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
          app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

          // Enable the application to use a cookie to store information for the signed in user
          // and to use a cookie to temporarily store information about a user logging in with a third party login provider
          // Configure the sign in cookie
          app.UseCookieAuthentication(new CookieAuthenticationOptions
          {
            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            LoginPath = new PathString("/Login"),
            Provider = new CookieAuthenticationProvider
            {
              OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, Guid>(
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentityCallback: (manager, user) => manager.GenerateUserIdentityAsync(user),
                    getUserIdCallback: (id) => (new Guid(id.GetUserId())))
            }
          });

          app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            GoogleOAuth2AuthenticationOptions googleOpt = new GoogleOAuth2AuthenticationOptions();
            googleOpt.ClientId = "928365693026-r7eoreit3e0fmnknh02ki8ivcf63qdj6.apps.googleusercontent.com";
            googleOpt.ClientSecret = "zKBFHJptabxBR3-0YbRELa2x";
            GoogleOAuth2AuthenticationProvider googleProv = new GoogleOAuth2AuthenticationProvider();
            googleProv.OnAuthenticated = async context =>
                {
                  string googleEmailAddress = context.Email;
                  string googleFirstName = context.GivenName;
                  string googleLastName = context.FamilyName;
                };
            googleOpt.Provider = googleProv;
            app.UseGoogleAuthentication(googleOpt);
        }
    }
}