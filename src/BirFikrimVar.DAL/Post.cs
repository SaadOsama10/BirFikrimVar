using System.ComponentModel.DataAnnotations;

namespace BirFikrimVar.DAL
{
    public partial class Post
    {
        public Post()
        {
            this.FeedbackMessage = new HashSet<FeedbackMessage>();
            this.PostPlot = new HashSet<PostPlot>();
            this.SavedPost = new HashSet<SavedPost>();
            this.PostLike = new HashSet<PostLike>();
        }

        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Tags { get; set; }

        public string Userd { get; set; }
        public DateTime? PostDate { get; set; }

        /// <summary>null = pending moderation, true = published, false = rejected.</summary>
        public bool? IsPublished { get; set; }

        public virtual ICollection<FeedbackMessage> FeedbackMessage { get; set; }
        public virtual ICollection<PostPlot> PostPlot { get; set; }
        public virtual ICollection<SavedPost> SavedPost { get; set; }
        public virtual ICollection<PostLike> PostLike { get; set; }
    }
}
