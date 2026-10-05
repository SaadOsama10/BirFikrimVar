namespace BirFikrimVar.DAL
{
    public partial class PostPlot
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public string Text { get; set; }
        public int? Sort { get; set; }
        public string ImageUrl { get; set; }

        public virtual Post Post { get; set; }
    }
}
