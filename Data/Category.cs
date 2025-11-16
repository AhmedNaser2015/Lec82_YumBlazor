using System.ComponentModel.DataAnnotations;

namespace Lec82_YumBlazor.Data
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter name..")]
        public string Name { get; set; }
    }
}
