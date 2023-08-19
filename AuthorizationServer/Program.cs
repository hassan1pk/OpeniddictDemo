using AuthorizationServer;
using AuthorizationServer.Data;
using AuthorizationServer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddControllersWithViews();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/account/login";

        })
        .AddCookie(IdentityConstants.ApplicationScheme)
        .AddCookie(IdentityConstants.ExternalScheme)
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"]!;
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            googleOptions.SignInScheme = IdentityConstants.ExternalScheme;

        })
        .AddMicrosoftAccount(microsoftOptions =>
        {
            microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"]!;
            microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"]!;
            microsoftOptions.SignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = configuration["Authentication:Facebook:AppId"]!;
            facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"]!;
            facebookOptions.SignInScheme = IdentityConstants.ExternalScheme;
        }); ;

services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
}).AddSignInManager()
.AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

services.AddDbContext<IdentityDbContext>(options =>
{
    // Configure the context to use an in-memory store.
    //options.UseInMemoryDatabase(nameof(IdentityDbContext));
    options.UseSqlite(builder.Configuration.GetConnectionString("OpenIddictDatabase"));

    // Register the entity sets needed by OpenIddict.
    options.UseOpenIddict();
});

services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentityDatabase"));    
});

services.AddOpenIddict()

        // Register the OpenIddict core components.
        .AddCore(options =>
        {
            // Configure OpenIddict to use the EF Core stores/models.
            options.UseEntityFrameworkCore()
                .UseDbContext<IdentityDbContext>();            
        })

        // Register the OpenIddict server components.
        .AddServer(options =>
        {
            options
                .AllowClientCredentialsFlow()                
                .AllowAuthorizationCodeFlow().RequireProofKeyForCodeExchange()
                .AllowPasswordFlow().RequireProofKeyForCodeExchange()
                .AllowRefreshTokenFlow();

            options
                .SetAuthorizationEndpointUris("/connect/authorize")
                .SetTokenEndpointUris("/connect/token")
                .SetUserinfoEndpointUris("/connect/userinfo");

            // Encryption and signing of tokens
            // USE THIS ON DEVELOPMENT ONLY
            // FOR PRODUCTION USE X.509 certificate
            options
                .AddEphemeralEncryptionKey()
                .AddEphemeralSigningKey()
                .DisableAccessTokenEncryption();

            // Register scopes (permissions)
            options.RegisterScopes("api");

            // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
            options
                .UseAspNetCore()
                .EnableTokenEndpointPassthrough()
                .EnableAuthorizationEndpointPassthrough()
                .EnableUserinfoEndpointPassthrough();
        });

services.AddHostedService<TestData>();

services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    
}

app.UseHttpsRedirection();
app.UseCors(cors =>
{
    cors.AllowAnyOrigin();
    cors.AllowAnyMethod();
    cors.AllowAnyHeader();
});
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Seed data
using (var scope = app.Services.CreateScope())
{
    var appServices = scope.ServiceProvider;
    var loggerFactory = appServices.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = appServices.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        var userManager = appServices.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = appServices.GetRequiredService<RoleManager<IdentityRole>>();
        await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
