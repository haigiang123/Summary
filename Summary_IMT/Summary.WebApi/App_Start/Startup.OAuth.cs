using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Summary.Model;
using Summary.Model.Models;
using Summary.WebApi.Infrastructure.Extension;
using System;

namespace Summary.WebApi.App_Start
{
    public partial class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider;

        public void ConfigureAuth(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(SummaryDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            //app.CreatePerOwinContext<EmailService>();
            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<UserManager<AppUser>>(CreateManager) ;

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(6),
                AllowInsecureHttp = true
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/Index"),
                CookieName = "Summary_Cookie",
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, AppUser>(
                        validateInterval: TimeSpan.FromMinutes(3),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),

                    //OnResponseSignIn = context =>
                    //{
                    //    context.Properties.AllowRefresh = true;
                    //    context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);
                    //},
                },
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
                SlidingExpiration = true,
            }) ;

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            
        }

        private static UserManager<AppUser> CreateManager(IdentityFactoryOptions<UserManager<AppUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<AppUser>(context.Get<SummaryDbContext>());
            var owinManager = new UserManager<AppUser>(userStore);

            return owinManager;
        }

        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    // Configure the db context, user manager and signin manager to use a single instance per request
        //    app.CreatePerOwinContext(SummaryDbContext.Create);
        //    app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        //    app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        //    //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        //    //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
        //    //app.CreatePerOwinContext<UserManager<AppUser>>(CreateManager);

        //    // Enable the application to use a cookie to store information for the signed in user
        //    // and to use a cookie to temporarily store information about a user logging in with a third party login provider
        //    // Configure the sign in cookie
        //    app.UseCookieAuthentication(new CookieAuthenticationOptions
        //    {
        //        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //        LoginPath = new PathString("/Account/Login"),
        //        Provider = new CookieAuthenticationProvider
        //        {
        //            // Enables the application to validate the security stamp when the user logs in.
        //            // This is a security feature which is used when you change a password or add an external login to your account.  
        //            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, AppUser>(
        //                validateInterval: TimeSpan.FromMinutes(30),
        //                regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        //        }
        //    });
        //    app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        //    // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
        //    app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

        //    // Enables the application to remember the second login verification factor such as phone or email.
        //    // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
        //    // This is similar to the RememberMe option when you log in.
        //    app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

        //    // Uncomment the following lines to enable logging in with third party login providers
        //    //app.UseMicrosoftAccountAuthentication(
        //    //    clientId: "",
        //    //    clientSecret: "");

        //    //app.UseTwitterAuthentication(
        //    //   consumerKey: "",
        //    //   consumerSecret: "");

        //    //app.UseFacebookAuthentication(
        //    //   appId: "",
        //    //   appSecret: "");

        //    //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
        //    //{
        //    //    ClientId = "",
        //    //    ClientSecret = ""
        //    //});
        //}
    }
}