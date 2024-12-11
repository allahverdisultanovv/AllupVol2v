namespace AllupVol2.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public List<ProductBrand> ProductBrands { set; get; }
    }
}
