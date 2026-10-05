using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BirFikrimVar
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                var cookie = HttpContext.Current?.Request?.Cookies["lang"];
                var lang = cookie != null ? cookie.Value : "en";  

                if (string.IsNullOrWhiteSpace(lang)) lang = "en";
                if (!lang.Equals("ar", StringComparison.OrdinalIgnoreCase) &&
                    !lang.Equals("en", StringComparison.OrdinalIgnoreCase))
                {
                    lang = "en";
                }

                string cultureName = lang.Equals("ar", StringComparison.OrdinalIgnoreCase) ? "ar-SY" : "en-US";
                var ci = CultureInfo.GetCultureInfo(cultureName);

                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
            catch
            {
            }
        }
    }
}