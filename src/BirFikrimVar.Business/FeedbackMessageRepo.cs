using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using Microsoft.EntityFrameworkCore;

namespace BirFikrimVar.Business
{
    public class FeedbackMessageRepo : IFeedbackMessageRepo
    {
        private readonly AppDbContext context;

        public FeedbackMessageRepo(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(FeedbackMessage feedbackMessage)
        {
            context.FeedbackMessages.Add(feedbackMessage);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            FeedbackMessage feedbackMessage = context.FeedbackMessages.Find(id);
            if (feedbackMessage == null) return;
            context.FeedbackMessages.Remove(feedbackMessage);
            context.SaveChanges();
        }

        public List<FeedbackMessage> GetAllFeedbackMessage(int postId)
        {
            return context.FeedbackMessages
                          .AsNoTracking()
                          .Include(p => p.Post)
                          .Where(fm => fm.PostId == postId)
                          .ToList();
        }

        public FeedbackMessage GetFeedbackMessageById(int id)
        {
            return context.FeedbackMessages.AsNoTracking().FirstOrDefault(f => f.Id == id);
        }

        public void Update(FeedbackMessage feedbackMessage)
        {
            FeedbackMessage old = context.FeedbackMessages.Find(feedbackMessage.Id);
            context.Entry(old).CurrentValues.SetValues(feedbackMessage);
            context.SaveChanges();
        }
    }
}
