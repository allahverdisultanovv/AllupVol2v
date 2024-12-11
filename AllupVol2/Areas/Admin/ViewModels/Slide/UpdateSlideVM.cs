namespace AllupVol2.Areas.Admin.ViewModels
{
    public class UpdateSlideVM
    {
        public string Maintitle { get; set; }
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
        public int Order { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string TitleDesc { get; set; }
    }
}
