using BirFikrimVar.DAL;
using Microsoft.EntityFrameworkCore;

namespace BirFikrimVar.Business
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext context;

        public PostRepo(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(Post post)
        {
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Post post = context.Posts.Find(id);
            if (post == null) return;
            context.Posts.Remove(post);
            context.SaveChanges();
        }

        public List<Post> GetAllPosts()
        {
            return context.Posts.AsNoTracking().Include(p => p.SavedPost).ToList();
        }

        public Post GetPostById(int id)
        {
            return context.Posts
                          .AsNoTracking()
                          .Include(p => p.PostPlot.OrderBy(pp => pp.Sort))
                          .FirstOrDefault(p => p.Id == id);
        }

        public void Update(Post post)
        {
            Post oldPost = context.Posts.Find(post.Id);
            context.Entry(oldPost).CurrentValues.SetValues(post);
            context.SaveChanges();
        }
    }
}
