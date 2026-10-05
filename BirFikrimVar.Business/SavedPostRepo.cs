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
    public class SavedPostRepo : ISavedPostRepo
    {
        public void Add(SavedPost savedPost)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                context.SavedPosts.Add(savedPost);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                SavedPost savedPost = context.SavedPosts.Find(id);
                context.SavedPosts.Remove(savedPost);
                context.SaveChanges(); 
            }
        }

        public List<SavedPost> GetAllSavedPosts(String userId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                List<SavedPost> savedPosts = context.SavedPosts
                    .Include(p => p.Post)
                    .Where(fm => fm.UserId == userId)
                    .ToList(); 
                return savedPosts;

            }
        }

        public SavedPost GetSavedPostById(int id)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                SavedPost savedPost = context
                                .SavedPosts.Find(id); 

                return savedPost;
            }
        }

        public void Update(SavedPost savedPost)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                SavedPost oldSavedPost = context.SavedPosts.Find(savedPost.Id);
                context.Entry(oldSavedPost).CurrentValues.SetValues(savedPost);
                context.SaveChanges();
            }
        }

        
    }
}

