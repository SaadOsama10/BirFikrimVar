using BirFikrimVar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BirFikrimVar.Business
{
    public class PostLikeRepo : IPostLikeRepo
    {
        public bool Like(string userd, int postId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                bool exists = context.PostLikes.Any(x => x.PostId == postId && x.Userd == userd);
                if (exists) return false;

                var like = new PostLike
                {
                    PostId = postId,
                    Userd = userd,
                    CreatedAt = DateTime.UtcNow
                };

                context.PostLikes.Add(like);
                context.SaveChanges();
                return true;
            }
        }

        public bool Unlike(string userd, int postId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                var like = context.PostLikes
                                  .FirstOrDefault(x => x.PostId == postId && x.Userd == userd);

                if (like == null) return false;

                context.PostLikes.Remove(like);
                context.SaveChanges();
                return true;
            }
        }

        public bool HasLiked(string userd, int postId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                return context.PostLikes.Any(x => x.PostId == postId && x.Userd == userd);
            }
        }

        public int CountLikes(int postId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                return context.PostLikes.Count(x => x.PostId == postId);
            }
        }

        public List<string> GetLikerIds(int postId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                return context.PostLikes
                              .Where(x => x.PostId == postId)
                              .OrderByDescending(x => x.CreatedAt)
                              .Select(x => x.Userd)
                              .ToList();
            }
        }
    }
}