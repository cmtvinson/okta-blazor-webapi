using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Okta.AspNetCore;
using okta_blazor_server_example;
using okta_blazor_server_example.Services;
using okta_core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<WeatherForecastApiService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<TokenProvider>();

#region Blazor Server-Side & Okta-Hosted Sign-In Page Example
// https://github.com/okta/samples-blazor/tree/master/server-side/okta-hosted-login
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddOktaMvc(new OktaMvcOptions
    {
        // Replace the Okta placeholders in appsettings.json with your Okta configuration.
        OktaDomain = builder.Configuration.GetValue<string>("Okta:OktaDomain"),
        ClientId = builder.Configuration.GetValue<string>("Okta:ClientId"),
        ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret"),
        AuthorizationServerId = builder.Configuration.GetValue<string>("Okta:AuthorizationServerId"),
    });
#endregion

#region ASP.NET Core Blazor Server additional security scenarios
// Pass tokens to a Blazor Server app
// https://docs.microsoft.com/en-us/aspnet/core/blazor/security/server/additional-scenarios?view=aspnetcore-6.0

builder.Services.Configure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;

        options.Scope.Add("offline_access");
    });
#endregion

// Customising claims transformation in ASP.NET Core Identity
// https://benfoster.io/blog/customising-claims-transformation-in-aspnet-core-identity/
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-6.0
builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
