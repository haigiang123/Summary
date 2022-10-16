using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using Summary.Model.Models;

using System.Security.Claims;
using System.Threading.Tasks;


namespace Summary.WebApi.Infrastructure.Extension
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            UserManager<AppUser> userManager = context.OwinContext.GetUserManager<UserManager<AppUser>>();
            AppUser user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                // Could not retrieve the user due to error.
                context.SetError("server_error", "Lỗi trong quá trình xử lý.");
                context.Rejected();
                return;
            }
            if (user != null)
            { 
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Password or UserName is incorrect");
                context.Rejected();
            }
        }
    }
}