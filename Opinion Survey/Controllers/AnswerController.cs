using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opinion_Survey.DTO;
using Opinion_Survey.Models;

namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        public AppDbContext _context;
        public AnswerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("FillForm")]
        public IActionResult FormAnswer(ListAnswerDto listAnswerDto)
        {
           
            foreach (var answer in listAnswerDto.Answers)
            {
                foreach (var text in answer.AnsText)
                {
                    Answer newAnswer = new Answer
                    {
                        QId = answer.QuestionId,
                        AnsText = text.ToString()
                    };
                    _context.Answers.Add(newAnswer);
                    _context.SaveChanges();
                }
               
            }

            return Ok();
        }
    }
}
/*[
    {
      "questionId": 3,
      "ansText": [

        "Good"
      ]
    },
    {
      "questionId": 4,
      "ansText": [

        "11","12"
      ]
    },
    {
      "questionId": 5,
      "ansText": [

        "10"
      ]
    }
  ]*/
