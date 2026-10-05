using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using Microsoft.EntityFrameworkCore;

namespace BirFikrimVar.Business
{
    public class PostLikeRepo : IPostLikeRepo
    {
        private readonly AppDbContext context;

        public PostLikeRepo(AppDbContext context)
        {
            this.context = context;
        }

        public bool Like(string userd, int postId)
        {
            if (context.PostLikes.Any(x => x.PostId == postId && x.Userd == userd)) return false;

            context.PostLikes.Add(new PostLike
            {
                PostId = postId,
                Userd = userd,
                CreatedAt = DateTime.UtcNow
            });
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // unique (PostId, Userd) index: a concurrent request already liked it
                return false;
            }
            return true;
        }

        public bool Unlike(string userd, int postId)
        {
            var like = context.PostLikes.FirstOrDefault(x => x.PostId == postId && x.Userd == userd);
            if (like == null) return false;
            context.PostLikes.Remove(like);
            context.SaveChanges();
            return true;
        }

        public bool HasLiked(string userd, int postId)
        {
            return context.PostLikes.Any(x => x.PostId == postId && x.Userd == userd);
        }

        public int CountLikes(int postId)
        {
            return context.PostLikes.Count(x => x.PostId == postId);
        }

        public List<string> GetLikerIds(int postId)
        {
            return context.PostLikes
                          .Where(x => x.PostId == postId)
                          .OrderByDescending(x => x.CreatedAt)
                          .Select(x => x.Userd)
                          .ToList();
        }
    }
}
