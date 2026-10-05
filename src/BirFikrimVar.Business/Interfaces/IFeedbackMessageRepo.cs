using BirFikrimVar.DAL;

namespace BirFikrimVar.Business.Interfaces
{
    public interface IFeedbackMessageRepo
    {
        void Add(FeedbackMessage feedbackMessage);
        void Delete(int id);
        List<FeedbackMessage> GetAllFeedbackMessage(int postId);
        FeedbackMessage GetFeedbackMessageById(int id);
        void Update(FeedbackMessage feedbackMessage);
    }
}
