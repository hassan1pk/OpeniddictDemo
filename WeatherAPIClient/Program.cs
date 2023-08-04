using OpenIddict.Client;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenIddict()
    .AddClient(options =>
    {
        options//.AllowAuthorizationCodeFlow()
        .AllowClientCredentialsFlow()
        .AllowPasswordFlow()
        .AllowRefreshTokenFlow();

        options.AddDevelopmentEncryptionCertificate()
        .AddDevelopmentSigningCertificate();

        options.UseAspNetCore();

        options.UseSystemNetHttp()
                       .SetProductInformation(typeof(Program).Assembly);

        options.AddRegistration(new OpenIddictClientRegistration
        {
            Issuer = new Uri("https://localhost:7033/", UriKind.Absolute),
            ClientId = "webapi",
            ClientSecret = "webapi-secret",
            //RedirectUri = new Uri("https://localhost:7188/", UriKind.Absolute)

           
        });        
    })
    .AddValidation(options=>
    {
        options.SetIssuer("https://localhost:7033/");
        // Register the ASP.NET Core host.
        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
