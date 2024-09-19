using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opinion_Survey.DTO;
using Opinion_Survey.Models;

namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        public AppDbContext _context;
        private readonly HttpClient _client;
        public AnswerController(AppDbContext context, HttpClient client)
        {
            _context = context;
            _client = client;

        }

        [HttpPost("FillForm")]
        public  async Task<IActionResult> FormAnswer(ListAnswerDto listAnswerDto)
        {
           
            foreach (var answer in listAnswerDto.Answers)
            {
                foreach (var text in answer.AnsText)
                {
                    Answer newAnswer = new Answer
                    {
                        QId = answer.QuestionId,
                        AnsText = text.ToString(),
                        AnalyzedAt = DateTime.Now
                    };

                    // Save Sentiment for paragraph answer
                    var type = _context.Questions.FirstOrDefault(x=> x.Id == answer.QuestionId);
                    if (type.Type.ToString() == "Paragraph")
                    {
                        var result = await AnalyzeSentiment(text);

                        if (result is BadRequestObjectResult badRequest)
                        {
                            return BadRequest(badRequest.Value);
                        }
                        var sentiment = result as OkObjectResult;
                        newAnswer.SentimentResult = sentiment?.Value.ToString();
                        
                    }
                    _context.Answers.Add(newAnswer);
                    _context.SaveChanges();
                }
               
            }
            return Ok();
        }


        /* Sentiment Model*/

        [HttpPost("analyze_sentiment")]
        [ApiExplorerSettings(IgnoreApi = true)] // This hides the endpoint in Swagger
        public async Task<IActionResult> AnalyzeSentiment(string text)
        {
            try
            {
                var url = "http://192.168.1.10:5000/analyze";
                var data = new { text = text };
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(result);
                    string sentiment = jsonResponse["sentiment_result"].ToString();
                    return Ok(sentiment);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return BadRequest($"Error in sentiment analysis: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
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
