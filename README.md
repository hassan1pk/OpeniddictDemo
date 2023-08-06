# OpenIddict Server Demo!

This is the code for the OpenIddict server. The server is created in .NET 7.0 and demonstrates the following OAuth 2.0 flows:

- Client Credentials Flow
- Authorization Code Flow (with PKCE)
- Password Credentials Flow

The server uses ASP.NET core identity and also supports the following **external providers**:

- Google
- Microsoft
- Facebook

## Databases

Both OpenIddict and ASP.NET core identity are configured to use separate SQLite databases.

When the project is started, the ASP.NET core identity is automatically seeded with a sample user.

## Client

.
There is also a WebAPI client named WeatherAPIClient created in .NET 7.0 to demonstrate how to use the OpenIddict server.

### Postman collection

There is also a postman collection that can be downloaded from [here](https://api.postman.com/collections/2187028-b011320f-ee77-4f4b-a659-92b5dffb62d3?access_key=PMAT-01H753MC7KAEV4EECDRXD8ZZGJ), which demonstrates how to get **access token** using the following flows:

- Client Credentials Flow
- Authorizaton Code Flow (with PKCE)
- Password Credentials Flow

## External providers

The app ids, secrets ids, and client secret ids are stored in the secret storage. You will have to generate your own secrets to test with external providers.

Refer to the following articles for generating secrets for different providers:

- [Google](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-7.0)
- [Microsoft](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-7.0)
- [Facebook](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-7.0)
