namespace BirFikrimVar.Services
{
    public interface IImageUploadService
    {
        /// <summary>Validates and stores an image; returns its public URL, or null if the file is not an acceptable image.</summary>
        Task<string> SaveAsync(IFormFile file);

        /// <summary>Removes a file previously returned by SaveAsync (used when a submission is rejected half-way).</summary>
        void Delete(string url);
    }

    public class ImageUploadService : IImageUploadService
    {
        public const long MaxBytes = 2 * 1024 * 1024;

        private readonly string uploadPath;

        public ImageUploadService(string uploadPath)
        {
            this.uploadPath = uploadPath;
            Directory.CreateDirectory(uploadPath);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null || file.Length == 0 || file.Length > MaxBytes) return null;

            // Trust the content, not the client-supplied name or Content-Type.
            var header = new byte[12];
            await using (var stream = file.OpenReadStream())
            {
                var read = 0;
                while (read < header.Length)
                {
                    var n = await stream.ReadAsync(header.AsMemory(read, header.Length - read));
                    if (n == 0) break;
                    read += n;
                }
                if (read < header.Length) return null;
            }

            var extension = Sniff(header);
            if (extension == null) return null;

            var name = Guid.NewGuid().ToString("N") + extension;
            await using (var target = File.Create(Path.Combine(uploadPath, name)))
            {
                await file.CopyToAsync(target);
            }
            return "/uploads/" + name;
        }

        public void Delete(string url)
        {
            if (string.IsNullOrEmpty(url) || !url.StartsWith("/uploads/")) return;
            var path = Path.Combine(uploadPath, Path.GetFileName(url));
            if (File.Exists(path)) File.Delete(path);
        }

        private static string Sniff(byte[] h)
        {
            if (h[0] == 0xFF && h[1] == 0xD8 && h[2] == 0xFF) return ".jpg";
            if (h[0] == 0x89 && h[1] == 0x50 && h[2] == 0x4E && h[3] == 0x47 &&
                h[4] == 0x0D && h[5] == 0x0A && h[6] == 0x1A && h[7] == 0x0A) return ".png";
            if (h[0] == 'G' && h[1] == 'I' && h[2] == 'F' && h[3] == '8' && (h[4] == '7' || h[4] == '9') && h[5] == 'a') return ".gif";
            if (h[0] == 'R' && h[1] == 'I' && h[2] == 'F' && h[3] == 'F' &&
                h[8] == 'W' && h[9] == 'E' && h[10] == 'B' && h[11] == 'P') return ".webp";
            return null;
        }
    }
}
