namespace AllupVol2.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Availability { get; set; }
        public string Title { get; set; }
        public decimal Tax { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }

        //relational
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductBrand> ProductBrands { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public List<ProductImage> ProductImages { get; set; }


    }
}
