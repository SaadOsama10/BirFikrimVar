using BirFikrimVar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Business.Interfaces
{
    public interface IFeedbackMessageRepo
    {
        void Add(FeedbackMessage feedbackMessage);
        void Update(FeedbackMessage feedbackMessage);
        void Delete(int  id);
        List<FeedbackMessage> GetAllFeedbackMessage(int postId);
        FeedbackMessage GetFeedbackMessageById(int id);
    }
}
