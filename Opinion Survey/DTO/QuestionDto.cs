using Opinion_Survey.Models;

namespace Opinion_Survey.DTO
{
    public class QuestionDto
    {
        public string QuestionText { get; set; }
        public TypeOfQuestion Type { get; set; }

        public IFormFile? ImageFile{ get; set; }
        public IFormFile? VideoFile { get; set; }
        public List<QuestionOptionDto> Options { get; set; }
    }
}
