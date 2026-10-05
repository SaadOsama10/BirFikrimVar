using System.Linq;
using System.Web.Mvc;
using BirFikrimVar.Business;
using BirFikrimVar.Business.Interfaces;  
using BirFikrimVar.DAL;
using BirFikrimVar.Resources;

namespace BirFikrimVar.Controllers
{
    public class LikeController : Controller
    {
        private readonly IPostLikeRepo postLikeRepo;

        public LikeController(IPostLikeRepo postLikeRepo)
        {
            this.postLikeRepo = postLikeRepo;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Toggle(int id) // id = PostId
        {
            var userId = UserHelper.GetCurrentUserId();
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
        public ActionResult Likers(int id)
        {
            var names = postLikeRepo.GetLikerIds(id)
                                    .Select(uid => UserHelper.GetUserNameById(uid))
                                    .ToList(); 

            return View(names); 
        }
    }
}