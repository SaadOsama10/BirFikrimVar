using BirFikrimVar.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirFikrimVar.Controllers
{
    public class HomeController : Controller
    {
        IPostRepo postRepo;

        public HomeController(IPostRepo postRepo)
        {
            this.postRepo = postRepo;
        }

        public IActionResult Index()
        {
            var model = postRepo
                        .GetAllPosts()
                        .Where(p => p.IsPublished.HasValue && p.IsPublished.Value)
                        .OrderByDescending(p => p.PostDate)
                        .ToList();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Search(string keywords)
        {
            keywords = (keywords ?? "").Trim();

            var model = postRepo
                .GetAllPosts()
                .Where(p =>
                    ((p.Title ?? "").Contains(keywords, StringComparison.OrdinalIgnoreCase)) ||
                    ((p.Tags ?? "").Contains(keywords, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();

            ViewBag.Keywords = keywords;
            return View(model);
        }
    }
}
