using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.Models
{
    public enum Type
    {
        SingleChoice = 1,
        MultipleChoice = 2,
        ShortAnswer = 3,
        Paragraph = 4,
        DropDown = 5
    }

    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public Type Type { get; set; }

        // Foreign Key to Form
        [ForeignKey("Forms")]
        public int FormId { get; set; }

        public virtual Form Forms { get; set; } // Changed from Forms to Form
    }
}
