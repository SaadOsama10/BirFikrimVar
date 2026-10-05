using BirFikrimVar.Business;
using BirFikrimVar.DAL;
using BirFikrimVar.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirFikrimVar.Areas.Admin.Controllers
{
    public class ManagePostController : Controller
    {
        IPostRepo postRepo;

        public ManagePostController(IPostRepo postRedo)
        {
            this.postRepo = postRedo;
        }
        // GET: Admin/ManagePost
        public ActionResult Index()
        {
            var posts = postRepo
                .GetAllPosts()
                .Where(p => !p.IsPublished.HasValue)
                .OrderBy(p => p.PostDate)
                .ToList();
            return View(posts);
        }

        public ActionResult Details(int id)
        {
            Post post = postRepo.GetPostById(id);
            return View(post);

        }
        public ActionResult Publish(int id)
        {
            Post post = postRepo.GetPostById(id);
            post.IsPublished = true;
            postRepo.Update(post);
            TempData["SuccessMessage"] = PostResources.PostPublishMessage;
            return RedirectToAction("Index");
    
        }

        public ActionResult Reject(int id)
        {
            Post post = postRepo.GetPostById(id);
            post.IsPublished = false; // أو null لو عايز ترجع البوست لقائمة الانتظار
            postRepo.Update(post);

            TempData["SuccessMessage"] = PostResources.PostRejectMessage;
            return RedirectToAction("Index");
        }
    }
}