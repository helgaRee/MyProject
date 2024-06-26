using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Helpers.Middlewares;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));








//IDENTITY
builder.Services.AddDefaultIdentity<UserEntity>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
    x.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<DataContext>();

//COOKIES
builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/signin"; //omdirigering om en sida inte fungerar
    x.LogoutPath = "/signout"; //omdirigering
    x.AccessDeniedPath = "/denied";

    x.Cookie.HttpOnly = true;//f�rhindrar risker f�r cross-side scripting
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.SlidingExpiration = true;


});




// Register services och repositories. Alla services och repos som anv datacintexten ska va scoped
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<FeatureService>();





builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<SubscribeRepository>();
builder.Services.AddScoped<FeatureRepository>();
builder.Services.AddScoped<FeatureItemRepository>();



var app = builder.Build();
app.UseHsts();
//dirigera sidor som inte kan hittas till error-sidan
app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");
//middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseUserSessionValidation();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
