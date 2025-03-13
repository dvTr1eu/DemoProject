using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Helpers
{
    public static class Utilities
    {
        public static string FormatDate(DateTime dateObj)
        {
            var formated = dateObj.ToString("dd-MM-yyyy");
            return formated;
        }

        public static string FormatPrice(int price)
        {
            var formated = price.ToString("##,###");
            return formated;
        }

        public static async Task<string?> UploadImageCloud(IFormFile imageUrl)
        {
            if (imageUrl == null || imageUrl.Length == 0)
            {
                return null;
            }

            var cloudinaryService = new CloudinaryService();
            var base64File = cloudinaryService.ConvertToBase64(imageUrl);
            var fileName = imageUrl.FileName;
            var uploadResult = await cloudinaryService.UploadImageAsync(base64File, fileName);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.AbsoluteUri;
            }

            return null;
        }
    }
}
