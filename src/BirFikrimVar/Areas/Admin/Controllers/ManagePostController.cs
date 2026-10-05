using BirFikrimVar.Business;
using BirFikrimVar.DAL;
using BirFikrimVar.Resources;
using BirFikrimVar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirFikrimVar.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DbSeeder.AdminRole)]
    public class ManagePostController : Controller
    {
        IPostRepo postRepo;

        public ManagePostController(IPostRepo postRepo)
        {
            this.postRepo = postRepo;
        }

        // GET: Admin/ManagePost
        public IActionResult Index()
        {
            var posts = postRepo
                .GetAllPosts()
                .Where(p => !p.IsPublished.HasValue)
                .OrderBy(p => p.PostDate)
                .ToList();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            Post post = postRepo.GetPostById(id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Publish(int id)
        {
            Post post = postRepo.GetPostById(id);
            if (post == null) return NotFound();
            post.IsPublished = true;
            postRepo.Update(post);
            TempData["SuccessMessage"] = PostResources.PostPublishMessage;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            Post post = postRepo.GetPostById(id);
            if (post == null) return NotFound();
            post.IsPublished = false; // use null instead to send the post back to the pending queue
            postRepo.Update(post);
            TempData["SuccessMessage"] = PostResources.PostRejectMessage;
            return RedirectToAction("Index");
        }
    }
}
