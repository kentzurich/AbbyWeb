using System.ComponentModel.DataAnnotations;

namespace AbbyWeb.Model
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Display Order")]
        [Range(1,100, ErrorMessage="Display Order must between 1 to 100.")]
        public int DisplayOrder { get; set; }
    }
}
