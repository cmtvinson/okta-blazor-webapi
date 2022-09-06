using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

// Customising claims transformation in ASP.NET Core Identity
// https://benfoster.io/blog/customising-claims-transformation-in-aspnet-core-identity/
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-6.0

namespace okta_blazor_server_example
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal is null) return default;

            var initialUser = (ClaimsIdentity)principal.Identity;

            if (initialUser.IsAuthenticated)
            {
                // Get async user info from DB and populate claims
                // this await is a placeholder for the DB call
                await Task.CompletedTask;

                // Example:
                // add authorization roles
                var claims = initialUser.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();

                if (!claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "Admin"))
                    (initialUser).AddClaim(new Claim(ClaimTypes.Role, "Admin"));

                if (!claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "Editor"))
                    (initialUser).AddClaim(new Claim(ClaimTypes.Role, "Editor"));

                // remove custom claim
                if (claims.Any(x => x.Type == "custom-claim1" && x.Value == "Hello There"))
                    (initialUser).RemoveClaim(new Claim("custom-claim1", "Hello There"));
            }

            // Build and return the new principal
            var newClaimsIdentity = new ClaimsIdentity(initialUser.Claims, initialUser.AuthenticationType);
            return new ClaimsPrincipal(newClaimsIdentity);
        }
    }
}
