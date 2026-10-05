using Microsoft.AspNetCore.Mvc;

namespace BirFikrimVar.Controllers
{
    public class LangController : Controller
    {
        [HttpGet]
        public IActionResult Set(string lang = "en", string returnUrl = "/")
        {
            lang = (lang ?? "en").ToLowerInvariant();
            if (lang != "en" && lang != "ar") lang = "en";
            if (!Url.IsLocalUrl(returnUrl)) returnUrl = "/";

            Response.Cookies.Append("lang", lang, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps
            });
            return LocalRedirect(returnUrl);
        }
    }
}
