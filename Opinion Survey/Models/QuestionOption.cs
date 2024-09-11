using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.Models
{
    public class QuestionOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public int Order {  get; set; }
        //public string Type { get; set; }
        public string OptionText { get; set; }

        [ForeignKey("Questions")]
        public int qId { get; set; }

        public virtual Question Questions { get; set; }
    }
}
