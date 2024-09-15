using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Opinion_Survey.DTO;
using Opinion_Survey.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailService;
        private readonly IHostingEnvironment _hosting;
       
        public AccountController(AppDbContext context,UserManager<User> userManager 
            , IConfiguration configuration ,IEmailSender emailSender,IHostingEnvironment hosting )
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailSender;
            _hosting = hosting;
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(ModifiedUserDTO newuser)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    FirstName = newuser.FirstName,
                    LastName = newuser.LastName,
                    UserName = newuser.Email, //Email
                    Age = newuser.Age,
                    Gender = newuser.Gender,
                    Email = newuser.Email,
                    PhoneNumber = newuser.PhoneNumber
                };
                string ImageFolderPath = Path.Combine(_hosting.WebRootPath, "Images");
                string NewImagePath = Path.Combine(ImageFolderPath, "Default.jpeg");
                //user.ImageFile.CopyTo(new FileStream(NewImagePath, FileMode.Create));
                user.Imagepath = NewImagePath;


                IdentityResult result = await _userManager.CreateAsync(user, newuser.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Build the confirmation link
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                        new { token, email = user.Email }, Request.Scheme);

                    // Send the confirmation email
                    await _emailService.SendEmailAsync(user.Email, "Confirm your account",
                        $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>");

                    return Ok(new { message = "Registration successful. Please check your email to confirm your account." });

                }
                else
                {
                    // Log the errors to understand why the creation failed
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                // Log the model state errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Consider logging errors or inspecting them
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return BadRequest(ModelState);
        
    }

        [HttpGet("ConfirmEmail")]
        [ApiExplorerSettings(IgnoreApi = true)] // This hides the endpoint in Swagger
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
                return BadRequest("Invalid token or email.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok(new { message = "Email confirmed successfully!" });
            }

            return BadRequest(result.Errors);
        }



        [HttpPost("logIn")]
        public async Task<IActionResult> LogIn(LogIn user)
        {
            User checkUser = await _userManager.FindByEmailAsync(user.Email);
           if(checkUser != null)
            {
                if(await _userManager.CheckPasswordAsync(checkUser, user.Password))
                {
                    // prepare claims in payload in JWT
                    var claims = new List<Claim>();


                    // custom claim
                    //claims.Add(new Claim("key", "value"));


                    // predefined claim
                    //claims.Add(new Claim(ClaimTypes.Name, checkUser.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, checkUser.Id));
                    // id for jwt
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


                    //Roles
                    var roles = await _userManager.GetRolesAsync(checkUser);
                    foreach(var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }
                    // Signing Credential [is Key] symmetric encription key (prepare key using symatricSecurityKey)
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                    var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                    // prepare Token
                    var token = new JwtSecurityToken(
                        claims: claims,
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: sc
                        );

                     // token object thar we send with responss and expiration Date
                    var _token = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };

                    return Ok(_token);

                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest("User Not Found");
        }
      
      



        

    }
}
