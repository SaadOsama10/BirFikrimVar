using BirFikrimVar.Business;
using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using BirFikrimVar.Models;
using BirFikrimVar.Resources;
using BirFikrimVar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirFikrimVar.Controllers
{
    public class PostController : Controller
    {
        IPostRepo postRepo;
        ISavedPostRepo savedPostRepo;
        IPostLikeRepo postLikeRepo;
        IUserDirectory users;
        IImageUploadService images;

        public PostController(IPostRepo postRepo, ISavedPostRepo savedPostRepo, IPostLikeRepo postLikeRepo,
                              IUserDirectory users, IImageUploadService images)
        {
            this.postRepo = postRepo;
            this.savedPostRepo = savedPostRepo;
            this.postLikeRepo = postLikeRepo;
            this.users = users;
            this.images = images;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Post post = postRepo.GetPostById(id);
            if (post == null) return NotFound();

            var userId = users.CurrentUserId;
            ViewBag.LikeCount = postLikeRepo.CountLikes(id);
            ViewBag.HasLiked = (userId != null) && postLikeRepo.HasLiked(userId, id);

            return View(post);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new CreatePostViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var post = new Post
            {
                Title = model.Title,
                Tags = model.Tags,
                PostDate = DateTime.UtcNow,
                Userd = users.CurrentUserId
            };

            // To stop accepting posts, and if I delete this, I will bring it back
            // post.IsPublished = true;

            foreach (var plot in model.PostPlot)
            {
                var postPlot = new PostPlot { Text = plot.Text, Sort = plot.Sort };
                if (plot.Image != null && plot.Image.Length > 0)
                {
                    postPlot.ImageUrl = await images.SaveAsync(plot.Image);
                }
                post.PostPlot.Add(postPlot);
            }

            postRepo.Add(post);
            TempData["SuccessMessage"] = PostResources.PostAddedMessage;
            return Redirect(Url.Content("~/"));
        }

        [Authorize]
        public IActionResult SavedPost(int id)
        {
            var savedpost = new SavedPost
            {
                PostId = id,
                UserId = users.CurrentUserId,
                Date = DateTime.UtcNow
            };
            savedPostRepo.Add(savedpost);
            TempData["SuccessMessage"] = PostResources.PostSavedMessage;
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult DeletePost(int savedPostId)
        {
            savedPostRepo.Delete(savedPostId);
            TempData["SuccessMessage"] = PostResources.DeletePostMessage;
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult SavedPosts()
        {
            var model = savedPostRepo.GetAllSavedPosts(users.CurrentUserId);
            return View(model);
        }
    }
}
