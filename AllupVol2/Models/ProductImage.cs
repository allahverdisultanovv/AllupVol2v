namespace AllupVol2.Models
{
    public class ProductImage : BaseEntity
    {
        public string ImageURL { get; set; }
        public bool? IsPrimary { get; set; }
        public Product product { get; set; }
    }
}
