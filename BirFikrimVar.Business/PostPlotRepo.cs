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
    public class PostPlotRepo : IPostPlotRepo 
    {
        public void Add(PostPlot postPlot)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                context.PostPlots.Add(postPlot);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                PostPlot postPlot = context.PostPlots.Find(id); 
                context.PostPlots.Remove(postPlot);
                context.SaveChanges();
            }
        }

        public List<PostPlot> GetAllPostPlot(int PostId)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                List<PostPlot> postPlots = context.PostPlots.Where(sp=> sp.PostId == PostId).ToList(); 
                return postPlots;

            }
        }


        public PostPlot GetPostPlotById(int id)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                PostPlot postPlot = context
                                .PostPlots
                                .Find(id);
                return postPlot;
            }
        }

        public void Update(PostPlot postPlot)
        {
            using (projectDBEntities context = new projectDBEntities())
            {
                PostPlot oldPostPlot = context.PostPlots.Find(postPlot.Id); 
                context.Entry(oldPostPlot).CurrentValues.SetValues(postPlot);
                context.SaveChanges();
            }
        }
    }
}
