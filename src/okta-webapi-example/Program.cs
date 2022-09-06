using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using okta_webapi_example;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Secure Your .NET 6 Web API
// How to Secure Your .NET Web API with Token Authentication - Get the API to Validate the Access Token
// https://developer.okta.com/blog/2018/02/01/secure-aspnetcore-webapi-token-auth#get-the-api-to-validate-the-access-token
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration.GetValue<string>("Okta:Issuer");
    options.Audience = "api://default";
    options.RequireHttpsMetadata = false;
});
#endregion

// Customising claims transformation in ASP.NET Core Identity
// https://benfoster.io/blog/customising-claims-transformation-in-aspnet-core-identity/
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-6.0
builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
