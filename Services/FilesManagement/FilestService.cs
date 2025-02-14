using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace e_commerce.Services.FilesManagement
{
    public class FilestService
    {
        private readonly Cloudinary _cloudinary;

        public FilestService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {

                if (file != null && file.Length > 0)
                {
                    if (file.Length > 1 * 1024 * 1024)  // 1 MB = 1 * 1024 * 1024 bytes
                    {
                        throw new InvalidOperationException("File size cannot be more than 1 MB.");
                    }


                    var allowedMimeTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp", "image/webp" };

                    if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                    {
                        throw new InvalidOperationException("Only jpeg,jpg,png,gif,bmp,webp image files are allowed.");
                    }

                    var guid = Guid.NewGuid();
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = $"{guid}/{Path.GetFileNameWithoutExtension(file.FileName)}",
                        Overwrite = true
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return uploadResult.SecureUrl.ToString(); // Return the Cloudinary URL
                    }
                }
                return "Images field is null";

            }
            catch(InvalidOperationException ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }
    }
}
