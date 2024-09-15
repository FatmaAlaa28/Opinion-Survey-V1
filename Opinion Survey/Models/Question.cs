using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.Models
{
    public enum TypeOfQuestion
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
        public TypeOfQuestion Type { get; set; }
        public string? Imagepath { get; set; }
       
        public string? Videopath { get; set; }
        // Foreign Key to Form
        [ForeignKey("Forms")]
        public int FormId { get; set; }

        public virtual Form Forms { get; set; } 

        public List<QuestionOption> Options { get; set; }
    }
}
