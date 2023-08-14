# OpenIddict Server Demo!

This is the code for the OAuth 2.0 and OpenID Connect server implemented using OpenIddict in .NET 7.0. The implementation demonstrates the following flows:

- Client Credentials Flow
- Authorization Code Flow (with PKCE)
- Password Credentials Flow

It also supports explicit consent type.

The server uses ASP.NET core identity. It also supports the following **external providers**:

- Google
- Microsoft
- Facebook

## Databases

Both OpenIddict and ASP.NET core identity are configured to use separate SQLite databases.

When the project is started, the ASP.NET core identity is automatically seeded with a sample user.

## Clients

### .NET 7 WebAPI client
There is a WebAPI client named WeatherAPI Client created in .NET 7.0 to demonstrate how to use the OpenIddict server.

### Postman collection

There is a postman collection that can be downloaded from [here](https://api.postman.com/collections/2187028-b011320f-ee77-4f4b-a659-92b5dffb62d3?access_key=PMAT-01H753MC7KAEV4EECDRXD8ZZGJ), which demonstrates how to get **access token** using the following flows:

- Client Credentials Flow
- Authorizaton Code Flow (with PKCE)
- Password Credentials Flow

There is also a request in postman collection that makes GET request to the WeatherAPI client to get the list of weather.

### React client
There is a client created in React with Typescript. The client demonstrates how to login using the Authorization Code Flow and then makes the GET request to the WeatherAPI to get the list of weather.

The client uses Redux to manage the state. The state is persisted in the browser's local storage. The state stores the access token and the refresh token.

The client uses Axios to fetch and post data using REST APIs.

The code makes use of Axios response interceptor to obtain the refresh token if HTTP error 401 is received when making the API call.

## External providers

The app ids, secrets ids, and client secret ids are stored in the secret storage. You will have to generate your own secrets to test with external providers.

Refer to the following articles for generating secrets for different providers:

- [Google](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-7.0)
- [Microsoft](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-7.0)
- [Facebook](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-7.0)

## To be done

- Sign up
- Email confirmation
- Two factor autentication (2FA)
- Other consent types
- Fix issue with facebook login
