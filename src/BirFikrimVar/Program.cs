using System.Globalization;
using BirFikrimVar.Business;
using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using BirFikrimVar.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// PaaS hosts tell the container which port to bind through $PORT.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port)) builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(ResolveConnectionString(builder.Configuration),
                      npgsql => npgsql.MigrationsAssembly("BirFikrimVar.DAL")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserDirectory, UserDirectory>();
builder.Services.AddScoped<IPostRepo, PostRepo>();
builder.Services.AddScoped<IPostLikeRepo, PostLikeRepo>();
builder.Services.AddScoped<ISavedPostRepo, SavedPostRepo>();
builder.Services.AddScoped<IFeedbackMessageRepo, FeedbackMessageRepo>();

var uploadPath = builder.Configuration["Uploads:Path"];
if (string.IsNullOrWhiteSpace(uploadPath))
    uploadPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "uploads");
builder.Services.AddSingleton<IImageUploadService>(new ImageUploadService(uploadPath));

builder.Services.AddControllersWithViews(options =>
{
    // Any POST/PUT/DELETE must carry a valid antiforgery token, even if an action forgets the attribute.
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
});

// Public demo with open registration: throttle write requests per client IP.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        if (HttpMethods.IsGet(context.Request.Method) || HttpMethods.IsHead(context.Request.Method))
            return RateLimitPartition.GetNoLimiter("read");
        return RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions { PermitLimit = 60, Window = TimeSpan.FromMinutes(1) });
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supported = new[] { new CultureInfo("en"), new CultureInfo("ar") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supported;
    options.SupportedUICultures = supported;
    // Same behaviour as the MVC 5 version: only the "lang" cookie decides, never the browser's Accept-Language.
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CustomRequestCultureProvider(context =>
    {
        var lang = context.Request.Cookies["lang"];
        return Task.FromResult(new ProviderCultureResult(lang == "ar" ? "ar" : "en"));
    }));
});

var app = builder.Build();

app.UseForwardedHeaders();

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Content-Security-Policy"] =
        "default-src 'self'; img-src 'self' data:; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
        "font-src https://fonts.gstatic.com; script-src 'self'; frame-ancestors 'none'; base-uri 'self'; form-action 'self'";
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.GetFullPath(uploadPath)),
    RequestPath = "/uploads"
});

app.UseRequestLocalization();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/healthz", () => Results.Ok("ok"));
app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
}

app.Run();

// Accepts a standard Npgsql connection string, or a postgres:// URL (the form most hosted PostgreSQL services hand out).
static string ResolveConnectionString(IConfiguration configuration)
{
    var raw = configuration.GetConnectionString("DefaultConnection") ?? configuration["DATABASE_URL"];
    if (string.IsNullOrWhiteSpace(raw))
        throw new InvalidOperationException(
            "No database configured. Set ConnectionStrings__DefaultConnection or DATABASE_URL (see .env.example).");

    if (raw.StartsWith("postgres://") || raw.StartsWith("postgresql://"))
    {
        var uri = new Uri(raw);
        var userInfo = uri.UserInfo.Split(':', 2);
        return new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : null,
            SslMode = SslMode.Require
        }.ConnectionString;
    }
    return raw;
}
