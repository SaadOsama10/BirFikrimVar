namespace BirFikrimVar.DAL
{
    public partial class SavedPost
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? PostId { get; set; }
        public DateTime? Date { get; set; }

        public virtual Post Post { get; set; }
    }
}
