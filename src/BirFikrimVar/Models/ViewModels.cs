using System.ComponentModel.DataAnnotations;

namespace BirFikrimVar.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class PostPlotInput
    {
        [StringLength(4000)]
        public string Text { get; set; }

        public int? Sort { get; set; }

        public IFormFile Image { get; set; }
    }

    /// <summary>Only the fields a visitor may set; IsPublished/Userd/PostDate are never bound from the form.</summary>
    public class CreatePostViewModel
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Tags { get; set; }

        public List<PostPlotInput> PostPlot { get; set; } = new List<PostPlotInput> { new PostPlotInput() };
    }

    public class SendCommentViewModel
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostAuthor { get; set; }

        [Required]
        [StringLength(2000)]
        public string Text { get; set; }
    }
}
