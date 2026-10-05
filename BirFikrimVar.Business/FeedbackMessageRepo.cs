using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Business
{
    public class FeedbackMessageRepo : IFeedbackMessageRepo 
    {
        public void Add(FeedbackMessage feedbackMessage)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                context.FeedbackMessages.Add(feedbackMessage);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                FeedbackMessage feedbackMessage = context.FeedbackMessages.Find(id); 
                context.FeedbackMessages.Remove(feedbackMessage);
                context.SaveChanges();
            }
        }

        public List<FeedbackMessage> GetAllFeedbackMessage(int postId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                List<FeedbackMessage> feedbackMessages = context.FeedbackMessages.Include(p => p.Post).Where(fm => fm.PostId == postId).ToList();
                return feedbackMessages;

            }
        }

        public FeedbackMessage GetFeedbackMessageById(int id) 
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                FeedbackMessage feedbackMessage = context
                                .FeedbackMessages.Find(id);

                return feedbackMessage;
            }
        }

        public void Update(FeedbackMessage feedbackMessage)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                FeedbackMessage oldFeedbackMessage = context.FeedbackMessages.Find(feedbackMessage.Id); 
                context.Entry(oldFeedbackMessage).CurrentValues.SetValues(feedbackMessage);
                context.SaveChanges();
            }
        }
    }
}

