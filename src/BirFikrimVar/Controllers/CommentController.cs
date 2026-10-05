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
    public class CommentController : Controller
    {
        IPostRepo postRepo;
        IFeedbackMessageRepo feedbackMessageRepo;
        IUserDirectory users;

        public CommentController(IPostRepo postRepo, IFeedbackMessageRepo commentRepo, IUserDirectory users)
        {
            this.postRepo = postRepo;
            this.feedbackMessageRepo = commentRepo;
            this.users = users;
        }

        [Authorize]
        public IActionResult Send(int id)
        {
            Post post = postRepo.GetPostById(id);
            if (post == null) return NotFound();

            return View(new SendCommentViewModel
            {
                PostId = id,
                PostTitle = post.Title,
                PostAuthor = users.GetUserName(post.Userd)
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Send(SendCommentViewModel model)
        {
            Post post = postRepo.GetPostById(model.PostId);
            if (post == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.PostTitle = post.Title;
                model.PostAuthor = users.GetUserName(post.Userd);
                return View(model);
            }

            feedbackMessageRepo.Add(new FeedbackMessage
            {
                PostId = model.PostId,
                Text = model.Text,
                Date = DateTime.UtcNow,
                UserId = users.CurrentUserId
            });
            TempData["SuccessMessage"] = PostResources.CommentSent;
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Inbox(int id)
        {
            // The inbox holds feedback for the post's author; admins can read it too.
            Post post = postRepo.GetPostById(id);
            if (post == null) return NotFound();
            if (!(users.IsAdmin || post.Userd == users.CurrentUserId)) return Forbid();

            var model = feedbackMessageRepo.GetAllFeedbackMessage(id);
            return View(model);
        }
    }
}
