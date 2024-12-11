namespace AllupVol2.Areas.Admin.ViewModels
{
    public class CreateSlideVM
    {
        public string Maintitle { get; set; }
        public IFormFile Image { get; set; }
        public int Order { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string TitleDesc { get; set; }
    }
}
