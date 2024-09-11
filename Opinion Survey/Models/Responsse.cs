using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.Models
{
    public class Responsse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public int Points { get; set; }

        [ForeignKey("Users")]
        public string UId { get; set; }

        [ForeignKey("Forms")]
        public int FormId { get; set; }

        [ForeignKey("Answers")]
        public int AnswerId { get; set; }


        public virtual User Users { get; set; }
        public virtual Form Forms { get; set; }
        public virtual Answer Answers { get; set; }
    }
}
