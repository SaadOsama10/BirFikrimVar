using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BirFikrimVar.DAL;
using BirFikrimVar.Business;
using Newtonsoft.Json;

namespace BirFikrimVar.Controllers
{

    public class HomeController : Controller
    {
        IPostRepo postRepo;
        public HomeController(IPostRepo postRepo)
        {
            this.postRepo = postRepo;
        }
        public ActionResult Index()
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
        public ActionResult Search(string keywords)
        {
            keywords = (keywords ?? "").Trim();

            var model = postRepo
                .GetAllPosts()
                .Where(p =>
                    ((p.Title ?? "").Contains(keywords)) ||
                    ((p.Tags ?? "").Contains(keywords))
                )
                .ToList();

            ViewBag.Keywords = keywords;
            return View(model);
        }

    }
}