using BirFikrimVar.Business.Interfaces;
using BirFikrimVar.DAL;
using Microsoft.EntityFrameworkCore;

namespace BirFikrimVar.Business
{
    public class SavedPostRepo : ISavedPostRepo
    {
        private readonly AppDbContext context;

        public SavedPostRepo(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(SavedPost savedPost)
        {
            context.SavedPosts.Add(savedPost);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            SavedPost savedPost = context.SavedPosts.Find(id);
            if (savedPost == null) return;
            context.SavedPosts.Remove(savedPost);
            context.SaveChanges();
        }

        public List<SavedPost> GetAllSavedPosts(string userId)
        {
            return context.SavedPosts
                          .AsNoTracking()
                          .Include(p => p.Post)
                          .Where(fm => fm.UserId == userId)
                          .ToList();
        }

        public SavedPost GetSavedPostById(int id)
        {
            return context.SavedPosts.AsNoTracking().FirstOrDefault(s => s.Id == id);
        }

        public void Update(SavedPost savedPost)
        {
            SavedPost oldSavedPost = context.SavedPosts.Find(savedPost.Id);
            context.Entry(oldSavedPost).CurrentValues.SetValues(savedPost);
            context.SaveChanges();
        }
    }
}
