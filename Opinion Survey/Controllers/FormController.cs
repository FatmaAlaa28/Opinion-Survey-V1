using Microsoft.AspNetCore.Http;
using Opinion_Survey.Models;
using Microsoft.AspNetCore.Mvc;
using Opinion_Survey.DTO;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FormController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _hosting;

        public FormController(AppDbContext context,IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }
        [HttpPost("create/{FolderId}")]
        public async Task<IActionResult> CreateForm([FromForm] FormDTO formDto, int? FolderId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Step 1: Create and save the Form
            var form = new Form
            {
                Title = formDto.Title,
                Description = formDto.Description,
                UserId = userId,
                FolderID = FolderId
            };

            _context.Forms.Add(form);
            await _context.SaveChangesAsync(); // Save form to get form.Id

            // Step 2: Add each Question and save it individually
            foreach (var questionDto in formDto.Questions)
            {
                var question = new Question
                {
                    QuestionText = questionDto.QuestionText,
                    Type = questionDto.Type,
                    FormId = form.Id
                };

                // Handle ImageFile for Question
                if (questionDto.ImageFile != null)
                {
                    string imageFolderPath = Path.Combine(_hosting.WebRootPath, "Images");
                    var imagePath = Path.Combine(imageFolderPath, questionDto.ImageFile.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await questionDto.ImageFile.CopyToAsync(stream);
                    }
                    question.Imagepath = imagePath;
                }

                // Handle VideoFile for Question
                if (questionDto.VideoFile != null)
                {
                    string videoFolderPath = Path.Combine(_hosting.WebRootPath, "Uploaded Video");
                    var videoPath = Path.Combine(videoFolderPath, questionDto.VideoFile.FileName);
                    using (var stream = new FileStream(videoPath, FileMode.Create))
                    {
                        await questionDto.VideoFile.CopyToAsync(stream);
                    }
                    question.Videopath = videoPath;
                }

                // Add the question to the context and save it
                _context.Questions.Add(question);
                await _context.SaveChangesAsync(); // Save question to get question.Id

                // Step 3: Add each QuestionOption for this question
                foreach (var optionDto in questionDto.Options)
                {
                    var option = new QuestionOption
                    {
                        OptionText = optionDto.OptionText,
                        qId = question.Id
                    };

                    _context.QuestionOptions.Add(option);
                }

                // Save all options for this question
                await _context.SaveChangesAsync();
            }

            return Ok("Form, questions, options, and media saved successfully.");
        }



    }
}
