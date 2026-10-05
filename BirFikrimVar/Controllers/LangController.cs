using System;
using System.Web;
using System.Web.Mvc;

namespace BirFikrimVar.Controllers
{
    public class LangController : Controller
    {
        [HttpGet]
        public ActionResult Set(string lang = "en", string returnUrl = "/")
        {
            lang = (lang ?? "en").ToLowerInvariant();
            if (lang != "en" && lang != "ar") lang = "en";

            if (!Url.IsLocalUrl(returnUrl)) returnUrl = "/";

            var cookie = new HttpCookie("lang", lang)
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(1),
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsSecureConnection
            };

            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }
    }
}