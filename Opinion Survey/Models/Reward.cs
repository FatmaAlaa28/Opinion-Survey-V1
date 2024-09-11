using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.Models
{
    public class Reward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; }
        public int Points { get; set; }
        public int NumOfSurveys { get; set; }
        [ForeignKey("Badges")]
        public int BId { get; set; }

        [ForeignKey("Users")]
        public string UId { get; set; }
        public virtual User Users { get; set; }
        public virtual Badge Badges { get; set; }
    }
}
