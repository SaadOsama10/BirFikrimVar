using BirFikrimVar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Business.Interfaces
{
    public interface IPostPlotRepo
    {
        void Add(PostPlot postPlot);
        void Update(PostPlot postPlot);
        void Delete(int id);
        List<PostPlot> GetAllPostPlot(int PostId);
        PostPlot GetPostPlotById(int id);
    }
}
