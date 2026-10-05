namespace BirFikrimVar.Business.Interfaces
{
    public interface IPostLikeRepo
    {
        bool Like(string userd, int postId);
        bool Unlike(string userd, int postId);
        bool HasLiked(string userd, int postId);
        int CountLikes(int postId);
        List<string> GetLikerIds(int postId);
    }
}
