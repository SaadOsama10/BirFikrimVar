using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BirFikrimVar.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<FeedbackMessage> FeedbackMessages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostPlot> PostPlots { get; set; }
        public virtual DbSet<SavedPost> SavedPosts { get; set; }
        public virtual DbSet<PostLike> PostLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>(e =>
            {
                e.Property(p => p.Userd).HasMaxLength(450);
                e.HasIndex(p => p.IsPublished);
            });
            builder.Entity<PostPlot>().Property(p => p.ImageUrl).HasMaxLength(500);
            builder.Entity<FeedbackMessage>().Property(p => p.UserId).HasMaxLength(450);
            builder.Entity<SavedPost>().Property(p => p.UserId).HasMaxLength(450);
            builder.Entity<PostLike>(e =>
            {
                e.Property(p => p.Userd).HasMaxLength(450).IsRequired();
                e.HasIndex(p => new { p.PostId, p.Userd }).IsUnique();
                e.HasOne(p => p.Post).WithMany(p => p.PostLike).HasForeignKey(p => p.PostId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
