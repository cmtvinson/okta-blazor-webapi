using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace okta_blazor_server_example.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        public IActionResult SignIn([FromQuery] string returnUrl)
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }

            return Challenge(OktaDefaults.MvcAuthenticationScheme);
        }

        [Authorize]
        public IActionResult SignOut([FromQuery] string returnUrl)
        {
            if (User.Identity is not null && !User.Identity.IsAuthenticated)
            {
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }

            return SignOut(
                new AuthenticationProperties() { RedirectUri = Url.Content("~/") },
                new[]
                {
                     OktaDefaults.MvcAuthenticationScheme,
                     CookieAuthenticationDefaults.AuthenticationScheme,
                });
        }
    }
}
