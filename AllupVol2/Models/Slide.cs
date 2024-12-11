namespace AllupVol2.Models
{
    public class Slide : BaseEntity
    {
        public string Maintitle { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string TitleDesc { get; set; }

    }
}
