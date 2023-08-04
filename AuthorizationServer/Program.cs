using AuthorizationServer;
using AuthorizationServer.Data;
using AuthorizationServer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/account/login";
            
        });



//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
//{
//    //Setting some configurations
//    config.User.RequireUniqueEmail = true;
//    config.Password.RequireNonAlphanumeric = false;
//    config.SignIn = new SignInOptions() { RequireConfirmedAccount = false, RequireConfirmedEmail = false, RequireConfirmedPhoneNumber = false };
//    //config.Cookies.ApplicationCookie.AutomaticChallenge = false;
//    //config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
//    //{
//    //    OnRedirectToLogin = context =>
//    //    {
//    //        if (context.Request.Path.StartsWithSegments("/api") &&
//    //        context.Response.StatusCode == 200)
//    //            context.Response.StatusCode = 401;
//    //        return Task.CompletedTask;
//    //    },
//    //    OnRedirectToAccessDenied = context =>
//    //    {
//    //        if (context.Request.Path.StartsWithSegments("/api") &&
//    //        context.Response.StatusCode == 200)
//    //            context.Response.StatusCode = 403;
//    //        return Task.CompletedTask;
//    //    }
//    //};
//})
//        .AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager()
//.AddDefaultTokenProviders();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{    
    options.Cookie = new CookieBuilder() { Name = ".AspNetCore.Cookies" };
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;

        return Task.CompletedTask;
    };
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    // Configure the context to use an in-memory store.
    //options.UseInMemoryDatabase(nameof(IdentityDbContext));
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentityDatabase"));

    // Register the entity sets needed by OpenIddict.
    options.UseOpenIddict();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));

    
});

builder.Services.AddOpenIddict()

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

builder.Services.AddHostedService<TestData>();

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
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        //await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
