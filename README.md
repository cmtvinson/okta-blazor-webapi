# Okta + BlazorServer + WebAPI 

## Objectives

### Blazor server 

- signin/signout to Okta
- implements ClaimsTransformer with example claims
- FetchData (WeatherForecast) uses WeatherForecastApiService
- WeatherForecastApiService sends Okta JWT to WebAPI as Bearer token

### WebAPI

- Validates/Authorizes incoming Okta JWT
- implements ClaimsTransformer with example claims
- WeatherForecastController implements [Authorize] attribute

&nbsp;  
  
---
## Requirements

- .Net 6 (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Visual Studio 2022 (https://visualstudio.microsoft.com/vs/)
- Okta Developer Account (https://developer.okta.com/signup/)

&nbsp;  

---  
## Configuration

### Set Okta values in dotnet user-secrets in Blazor Server project:

`cd src/okta-blazor-server-example`  
`dotnet user-secrets set "Okta:Domain" "https://{account}.okta.com"`  
`dotnet user-secrets set "Okta:ClientId" "{ClientId}"`  
`dotnet user-secrets set "Okta:ClientSecret" "{ClientSecret}"`  
`dotnet user-secrets set "Okta:AuthorizationServerId" "default"`  

### Set Okta values in dotnet user-secrets in WebAPI project:

`cd src/okta-webapi-example`  
`dotnet user-secrets init`  
`dotnet user-secrets set "Okta:Issuer" "https://{account}.okta.com/oauth2/default"`  

&nbsp;  

---
## Build

run build.bat from command line  
or  
click_to_build.bat from file explorer

&nbsp;  

---
## Helpful Links

- https://github.com/okta/samples-blazor/tree/master/server-side/okta-hosted-login  
- https://docs.microsoft.com/en-us/aspnet/core/blazor/security/server/additional-scenarios?view=aspnetcore-6.0  
- https://benfoster.io/blog/customising-claims-transformation-in-aspnet-core-identity/
- https://docs.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-6.0
- https://developer.okta.com/blog/2018/02/01/secure-aspnetcore-webapi-token-auth#get-the-api-to-validate-the-access-token

&nbsp;  

---
## Notes

Make sure both the server and api projects are set as startup projects when debugging
