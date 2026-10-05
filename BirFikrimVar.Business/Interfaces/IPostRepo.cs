using BirFikrimVar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Business
{
    public interface IPostRepo
    {
        void Add(Post post);
        void Update(Post post);
        void Delete(int id);
        List<Post> GetAllPosts();
        Post GetPostById(int id);

    }
}
