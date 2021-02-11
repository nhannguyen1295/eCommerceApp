using System.IO;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Entities.DTO
{
    public class ProductMediaDTO
    {
        public MediaType Type { get; set; }
        public FileStream File { get; set; }
    }
}