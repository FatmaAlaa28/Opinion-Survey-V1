using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_Survey.DTO
{
    public class AnswerDto
    {
        
        public int QuestionId { get; set; }
        public List<string> AnsText { get; set; }
      
    }
}
