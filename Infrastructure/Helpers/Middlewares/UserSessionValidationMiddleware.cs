using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Helpers.Middlewares;

//middleware som validerar om det finns en user i databasen med kontroinformationen som finns i sessionen
public class UserSessionValidationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private static bool IsAjaxRequest(HttpRequest request) => request.Headers["XRequestedWith"] == "XMLHttpRequest";

    public async Task InvokeAsync(HttpContext context, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
    {
        //kontroll om en session finns inne
        if (context.User.Identity!.IsAuthenticated)
        {

            var user = await userManager.GetUserAsync(context.User);
            if (user == null)
            {
                //om den är tom, förgör sessionen
                await signInManager.SignOutAsync();

                //kontrollera så att det inte är ett ajaxrequest och att det är en GET-metod som används
                if (!IsAjaxRequest(context.Request) && context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    var signInPath = "/signin";
                    context.Response.Redirect(signInPath);
                    return;
                }
            }



        }
        await _next(context);
    }


}
