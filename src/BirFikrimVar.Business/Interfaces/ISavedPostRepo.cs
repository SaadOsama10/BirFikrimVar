using BirFikrimVar.DAL;

namespace BirFikrimVar.Business.Interfaces
{
    public interface ISavedPostRepo
    {
        void Add(SavedPost savedPost);
        void Delete(int id);
        List<SavedPost> GetAllSavedPosts(string userId);
        SavedPost GetSavedPostById(int id);
        void Update(SavedPost savedPost);
    }
}
