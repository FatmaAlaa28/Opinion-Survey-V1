using Microsoft.AspNetCore.Http;
using Opinion_Survey.Models;
using Microsoft.AspNetCore.Mvc;
using Opinion_Survey.DTO;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FormController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FormController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateForm([FromBody] FormModified inputform)
        {
            if (inputform == null)
            {
                return BadRequest(new { message = "Input form cannot be null" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Initialize form
            var form = new Form
            {
                Title = inputform.Title,
                Description = inputform.Description
            };

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);
                var userId = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token." });
                }

                form.UserId = userId;
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Invalid token", details = ex.Message });
            }

            // Save form to the database
            try
            {
                _context.Forms.Add(form);
                _context.SaveChanges(); // Save to get the form.Id
                //return Ok(form.Id);
                // Initialize question object
                Question question = new Question
                {
                    QuestionText = inputform.QuestionText,
                    Type = inputform.Type,
                    FormId = form.Id // FormId is available now after SaveChanges
                };

                _context.Questions.Add(question);
                _context.SaveChanges();

                return Ok(new { message = "Form and question created successfully", formId = form.Id, questionId = question.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the form and question", details = ex.Message });
            }
        }
    }
}
