using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BirFikrimVar.Business;
using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using BirFikrimVar.Resources;

namespace BirFikrimVar.Controllers
{
    public class CommentController : Controller
    {
        IPostRepo postRepo;
        IFeedbackMessageRepo feedbackMessageRepo;
        public CommentController(IPostRepo postRepo, IFeedbackMessageRepo commentRepo)
        {
            this.postRepo = postRepo;
            this.feedbackMessageRepo = commentRepo;
        }

        [Authorize]
        public ActionResult Send(int id)
        {
            FeedbackMessage comment = new FeedbackMessage();
            comment.PostId = id;
            comment.Post = postRepo.GetPostById(id);
            return View(comment);

        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Send(FeedbackMessage model)
        {
            model.Date = DateTime.Now;
            model.UserId = UserHelper.GetCurrentUserId();
            feedbackMessageRepo.Add(model);
            TempData["SuccessMessage"] = PostResources.CommentSent;
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult Inbox(int id)
        {
            var model = feedbackMessageRepo.GetAllFeedbackMessage(id);
            return View(model);
        }
    }
}