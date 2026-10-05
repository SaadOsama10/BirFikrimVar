using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.Resources;
using BirFikrimVar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirFikrimVar.Controllers
{
    public class LikeController : Controller
    {
        private readonly IPostLikeRepo postLikeRepo;
        private readonly IUserDirectory users;

        public LikeController(IPostLikeRepo postLikeRepo, IUserDirectory users)
        {
            this.postLikeRepo = postLikeRepo;
            this.users = users;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int id) // id = PostId
        {
            var userId = users.CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (postLikeRepo.HasLiked(userId, id))
            {
                postLikeRepo.Unlike(userId, id);
                TempData["SuccessMessage"] = PostResources.LikeRemoved;
            }
            else
            {
                postLikeRepo.Like(userId, id);
                TempData["SuccessMessage"] = PostResources.LikeAdded;
            }
            return RedirectToAction("Details", "Post", new { id });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Likers(int id)
        {
            var names = postLikeRepo.GetLikerIds(id)
                                    .Select(uid => users.GetUserName(uid))
                                    .ToList();
            return View(names);
        }
    }
}
