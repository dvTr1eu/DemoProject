using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary cloudinary;
        public CloudinaryService()
        {
            Account account = new Account(
                "dblgdqkqo",  // cloud_name
                "586632876713348",  // api_key
                "ZgzfPc0lGEhuLPGhH30Oi9uHWPc"  // api_secret
            );
            cloudinary = new Cloudinary(account);
        }
        public async Task<ImageUploadResult> UploadImageAsync(string base64File,string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("data:image/png;base64," + base64File),
                Folder = "Movie",
                PublicId = Path.GetFileNameWithoutExtension(fileName)
            };
            return await cloudinary.UploadAsync(uploadParams);
        }

        public string ConvertToBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
    }
}
