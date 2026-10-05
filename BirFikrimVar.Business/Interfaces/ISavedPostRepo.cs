using BirFikrimVar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Business.Interfaces
{
    public interface ISavedPostRepo
    {
        void Add(SavedPost savedPost);
        void Update(SavedPost savedPost);
        void Delete(int id);
        List<SavedPost> GetAllSavedPosts(String userId);
        SavedPost GetSavedPostById(int id);
    }
}
