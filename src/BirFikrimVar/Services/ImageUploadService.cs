namespace BirFikrimVar.Services
{
    public interface IImageUploadService
    {
        /// <summary>Stores an uploaded image under a unique name and returns its public URL.</summary>
        Task<string> SaveAsync(IFormFile file);
    }

    public class ImageUploadService : IImageUploadService
    {
        private readonly string uploadPath;

        public ImageUploadService(string uploadPath)
        {
            this.uploadPath = uploadPath;
            Directory.CreateDirectory(uploadPath);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            string original = Path.GetFileName(file.FileName);
            string unique = Path.GetFileNameWithoutExtension(original)
                            + "_" + Guid.NewGuid().ToString("N")
                            + Path.GetExtension(original);

            await using (var target = File.Create(Path.Combine(uploadPath, unique)))
            {
                await file.CopyToAsync(target);
            }
            return "/uploads/" + unique;
        }
    }
}
