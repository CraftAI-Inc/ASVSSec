using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

public class LoginModel : PageModel
{
    public async Task<IActionResult> OnPostAsync(string username)
    {
        // Simple demo logic: No password check, just create a cookie for the username
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return RedirectToPage("/Index");
    }
}