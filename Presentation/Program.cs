using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Helpers.Middlewares;
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

    x.Cookie.HttpOnly = true;//förhindrar risker för cross-side scripting
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.SlidingExpiration = true;


});


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
