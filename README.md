# Overview
Sample ASP.NET Core Web API for the *Securing and Testing REST APIs in ASP.NET Core* session at Orlando Code Camp 2025

## Details
This solution was built using .NET 9.0 and ASP.NET Core 9.0 with Sqlite for the backend databases.  It consists of the following projects:

- WebAPI:  host service for the minimal APIs that perform basic CRUD operations for the Album entity exposed by the data access project via the services layer.  The database supporting the API is contained in the App_Data subfolder and can be created using the SQL script here:  https://github.com/lerocha/chinook-database/blob/master/ChinookDatabase/DataSources/Chinook_Sqlite.sql.  If you choose to locate the database elsewhere, just make sure to update the connection string in appsettings.json.  EF Core provides the ORM to interface with the database.
- WebAPI.DataAccess:  contains the DbContext, models and repository exposing the Album entity specifically.  Other entities are available, but not currently exposed.
- WebAPI.Services:  provides the interface between the Web APIs and the data access layer by exposing DTOs to the caller.
- AuthService:  sample Duende IdentityServer used to issue access tokens to either an interactive client using the authorization code flow or a service client using the client credentials flow.  This service also has its own Sqlite database which is created using the included standup_db.bat file.

## Getting Started
Start by creating the two Sqlite databases referenced above.  Verify connectivity to each.  Each database comes pre-seeded with data and should work out of the box without any other scripting required.

Set the startup project(s) depending on what you want to test.  Run the WebAPI project if you want to exercise the APIs using the user-jwts CLI to issue tokens and use these for access.  Alternatly, run both the WebAPI and AuthService projects if you want to issue tokens with the AuthService using Google or Duende as the identity provider.

## Securing the API
A variety of methods have been used to secure the APIs as outlined in the live session.  These include:

- Using Bearer authentication with tokens issued either via the user-jwts CLI or via the included AuthService
- Using scopes for authorization to control access to the APIs endpoints
- Using input model validation with FluentValidation
- Using ASP.NET Core's exception handling middleware
- Using rate limiting with ASP.NET Core

## Other features
Additional features for the sample web API include:

- Documenting the API with ASP.NET Core's support for the Open API standard and exposing this with Scalar
- Versioning the API with Microsoft's Asp.Versioning.Http library
- A starting point for DTO to entity mapping using AutoMapper
- Included sample for an IEndpointFilter implementation used with minimal APIs.  Here for illustrative purposes only to contrast that with the use of FluentValidation

## Testing the API
The main focus on testing with this sample API is functional.  Since the API is secured using bearer authentication with JWTs, these can be generated locally using the user-jwts tool then plugged into the WebAPI.http file used to invoke various methods on the APIs.  Make sure to set the correct scope when creating the JWT with user-jwts.  For example:

dotnet user-jwts create --scope "web-api:read" --valid-for 1h

The resulting decoded JWT should be similar to:

{
  "unique_name": "user12345",
  "sub": "user12345",
  "jti": "1cfc28c7",
  "scope": "web-api:read",
  "aud": [
    "http://localhost:5283",
    "https://localhost:7093"
  ],
  "nbf": 1743788145,
  "exp": 1751650545,
  "iat": 1743788146,
  "iss": "dotnet-user-jwts"
}

The above token will work for GET operations on the APIs.  Similarly, for PUT, POST and DELETE operations, generate a JWT using the "web-api:admin" scope.

Access tokens can also be generated via Postman using the sample collection provided.  These tokens can be generated either via the client credentials flow and the 'm2m' client Id and secret.  A token can also be generated interactively using the authoration code flow with PKCE and the 'interactive' client Id and secret.  Refer to the Config.cs class in the AuthService project to get more information on the clients being used.

## Resources
Open API: https://aka.ms/aspnet/openapi

API authorization:
- https://learn.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api#authorization
- https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-9.0

Rate limiting:  https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-7.0

Fluent validation:  https://docs.fluentvalidation.net/en/latest/aspnet.html

API versioning:  https://weblogs.asp.net/ricardoperes/asp-net-core-api-versioning

Testing APIs with .http files:  https://learn.microsoft.com/en-us/aspnet/core/test/http-files?view=aspnetcore-9.0

dotnet user-jwts CLI:  https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-9.0&tabs=windows

Postman:  https://learning.postman.com/docs/introduction/overview/

Scalar:  
- https://scalar.com/
- https://blog.antosubash.com/posts/dotnet-openapi-with-scalar

## License
Licensed under the BSD-2-Clause
