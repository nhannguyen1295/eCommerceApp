using System.IO;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace eCommerceApp.Server.Extensions
{
    public static class FileExtension
    {
        public const string DATAPATH = "E:\\Databases\\eCommerce\\Media";

        public static async Task SaveFileAsync(this IFormFile formFile, MediaType mediaType, string fileName)
        {
            var path = mediaType == MediaType.Picture ? Path.Combine(DATAPATH, "Pictures") : Path.Combine(DATAPATH, "Videos");
            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
        }
    }
}