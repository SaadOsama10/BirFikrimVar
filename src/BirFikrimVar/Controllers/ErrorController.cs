using Microsoft.AspNetCore.Mvc;

namespace BirFikrimVar.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{code?}")]
        public IActionResult Index(int? code)
        {
            Response.StatusCode = code ?? 500;
            return View(code ?? 500);
        }
    }
}
