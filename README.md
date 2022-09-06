# Okta + BlazorServer + WebApi 

Blazor server 
- signin/signout to Okta
- implements ClaimsTransformer with example claims
- FetchData (WeatherForecast) uses WeatherForecastApiService
- WeatherForecastApiService sends Okta JWT to WebAPI as Bearer token

WebAPI
- Validates/Authorizes incoming Okta JWT
- implements ClaimsTransformer with example claims
- WeatherForecastController implements [Authorize] attribute

---
Replace Okta values in:
- okta-blazor-server-example/appsettings.json
- okta-webapi-example/appsettings.json

---
run build.bat from command line

or

click_to_build.bat from file explorer

