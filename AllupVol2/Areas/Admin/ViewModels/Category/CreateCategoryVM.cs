using System.ComponentModel.DataAnnotations;

namespace AllupVol2.Areas.Admin.ViewModels
{
    public class CreateCategoryVM
    {
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; }

    }
}
