using BirFikrimVar.DAL;

namespace BirFikrimVar.Business
{
    public interface IPostRepo
    {
        void Add(Post post);
        void Delete(int id);
        List<Post> GetAllPosts();
        Post GetPostById(int id);
        void Update(Post post);
    }
}
