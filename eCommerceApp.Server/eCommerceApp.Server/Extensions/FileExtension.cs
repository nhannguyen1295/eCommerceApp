using System;
using System.IO;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace eCommerceApp.Server.Extensions
{
    public static class FileExtension
    {
        private const string DATAPATH = "E:\\Databases\\eCommerce\\Media";

        public static async Task SaveFileAsync(this IFormFile formFile, MediaType mediaType, string fileName)
        {
            var path = GetTruePath(mediaType);
            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
        }

        public static string GetTruePath(MediaType mediaType)
        => mediaType == MediaType.Picture ? Path.Combine(DATAPATH, "Pictures") : Path.Combine(DATAPATH, "Videos");

        public static string ConvertFileToBase64String(ProductMedia productMedia, MediaType mediaType)
        {
            var fullFileName = String.Join(".", productMedia.FileName, productMedia.FileExtension);
            var mime = String.Join("/", "application", productMedia.FileExtension);
            Byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(GetTruePath(mediaType), fullFileName));
            return Convert.ToBase64String(bytes);
        }
    }
}