namespace BirFikrimVar.DAL
{
    public partial class PostLike
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Userd { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post Post { get; set; }
    }
}
