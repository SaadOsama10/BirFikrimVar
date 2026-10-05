using BirFikrimVar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BirFikrimVar.Business
{
    public class PostRepo : IPostRepo
    {
        public void Add(Post post)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                context.Posts.Add(post);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                Post post = context.Posts.Find(id);
                context.Posts.Remove(post);
                context.SaveChanges();
            }
        }

        public List<Post> GetAllPosts()
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                List<Post> posts = context.Posts.Include(p=> p.SavedPost).ToList();
                return posts;
                
            }
        }

        public Post GetPostById(int id)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                Post post = context
                                .Posts
                                .Where(p => p.Id == id)
                                .Include(p => p.PostPlot)
                                .FirstOrDefault();
                return post;
            }
        }

        public void Update(Post post)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                Post oldPost = context.Posts.Find(post.Id);
                context.Entry(oldPost).CurrentValues.SetValues(post);
                context.SaveChanges();
            }
        }
    }
}
