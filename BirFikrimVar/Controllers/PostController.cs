using BirFikrimVar.Business;
using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using BirFikrimVar.Resources;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace BirFikrimVar.Controllers
{
    public class PostController : Controller
    {
        IPostRepo postRepo;
        ISavedPostRepo savedPostRepo;
        IPostLikeRepo postLikeRepo; 

        public PostController(IPostRepo postRepo, ISavedPostRepo savedPostRepo, IPostLikeRepo postLikeRepo) 
        {
            this.postRepo = postRepo;
            this.savedPostRepo = savedPostRepo;
            this.postLikeRepo = postLikeRepo; 
        }

        
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            Post post = postRepo.GetPostById(id);

            
            var userId = User.Identity.IsAuthenticated ? UserHelper.GetCurrentUserId() : null;
            ViewBag.LikeCount = postLikeRepo.CountLikes(id);
            ViewBag.HasLiked = (userId != null) && postLikeRepo.HasLiked(userId, id);
       

            return View(post);
        }

        [Authorize]
        public ActionResult Create()
        {
            Post post = new Post();
            post.PostPlot = new List<PostPlot>();
            post.PostPlot.Add(new PostPlot());
            return View(post);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Post model, IEnumerable<HttpPostedFileBase> files)
        {
            model.PostDate = DateTime.Now;
            model.Userd = UserHelper.GetCurrentUserId();
            int i = 0;

            // To stop accepting posts, and if I delete this, I will bring it back
            // model.IsPublished = true; 
            
            foreach (PostPlot postPlot in model.PostPlot)
            {
                if (files != null && files.Count() > i && files.ElementAt(i) != null)
                {
                    var file = files.ElementAt(i);
                    string original = System.IO.Path.GetFileName(file.FileName);
                    string unique = System.IO.Path.GetFileNameWithoutExtension(original)
                                    + "_" + Guid.NewGuid().ToString("N")
                                    + System.IO.Path.GetExtension(original);

                    string path = System.IO.Path.Combine(Server.MapPath("~/Images"), unique);
                    file.SaveAs(path);

                    if (!string.IsNullOrEmpty(unique))
                    {
                        postPlot.ImageUrl = "/Images/" + unique;
                    }
                }

                i++;

            }
            PostRepo postRepo = new PostRepo();
            postRepo.Add(model);
            TempData["SuccessMessage"] = PostResources.PostAddedMessage;
            return Redirect(Url.Content("~/"));

        }

        [Authorize]
        public ActionResult SavedPost(int id)
        {
            var savedpost = new SavedPost
            {
                PostId = id,
                UserId = UserHelper.GetCurrentUserId(),
                Date = DateTime.Now
            };
            savedPostRepo.Add(savedpost);
            TempData["SuccessMessage"] = PostResources.PostSavedMessage;
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult DeletePost(int savedPostId)
        {
            savedPostRepo.Delete(savedPostId);
            TempData["SuccessMessage"] = PostResources.DeletePostMessage;
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult SavedPosts()
        {
            var model = savedPostRepo.GetAllSavedPosts(UserHelper.GetCurrentUserId());
            return View(model);
        }
    }
}