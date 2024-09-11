using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AnsText { get; set; }

        [ForeignKey("Questions")]
        public int QId { get; set; }

        public virtual Question Questions { get; set; }
    }
}
