namespace AllupVol2.Models
{
    public class ProductBrand
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int BrandId { get; set; }
        public Product Product { get; set; }
        public Brand Brand { get; set; }

    }
}
