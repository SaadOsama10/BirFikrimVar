using System.ComponentModel.DataAnnotations;

namespace BirFikrimVar.DAL
{
    public partial class FeedbackMessage
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public int? PostId { get; set; }
        public string UserId { get; set; }

        public virtual Post Post { get; set; }
    }
}
