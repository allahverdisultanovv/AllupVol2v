using AllupVol2.Models;
using System.ComponentModel.DataAnnotations;

namespace AllupVol2.Areas.Admin.ViewModels
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public bool Availability { get; set; }
        public string Title { get; set; }
        public decimal Tax { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Category daxil et")]
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
        public List<int>? TagIds { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<int>? BrandIds { get; set; }
        public List<Brand> Brands { get; set; }
    }
}
