using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.Models
{
    public class Badge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotMapped]
        public IFormFile Picture { get; set; }

        public string PhotoPath {  get; set; }
        public int Point { get; set; }
    }
}
