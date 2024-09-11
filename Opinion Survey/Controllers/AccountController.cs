using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Opinion_Survey.DTO;
using Opinion_Survey.Models;

namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public AccountController(AppDbContext context,UserManager<User> userManager , IConfiguration configuration )
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
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
            
                IdentityResult result = await _userManager.CreateAsync(user, newuser.Password);

                if (result.Succeeded)
                {
                    return Ok("Success");
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
        

        //if(ModelState.IsValid)
        //{
        //    User user = new User();
        //    user.Email = newuser.Email;
        //    user.Password = newuser.Password;   
        //    user.PhoneNumber = newuser.PhoneNumber;
        //    user.Gender = newuser.Gender;
        //    user.Age = newuser.Age;
        //    user.Role = newuser.Role;
        //    user.ConfirmPassword = newuser.ConfirmPassword;
        //    user.EducationState = newuser.EducationState;
        //    user.Name = newuser.Name;
        //    _context.Add(user);

        //    _context.SaveChanges();
        //    return StatusCode(StatusCodes.Status204NoContent);
        //}
    }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest(new { message = "Invalid email confirmation request." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok(new { message = "Email confirmed successfully." });
            }

            return BadRequest(new { message = "Email confirmation failed.", errors = result.Errors });
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
            return BadRequest();
        }
      
      



        [HttpGet("GetProfile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            GetProfile getUser = new();
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userId = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token." });
                }
                User user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    getUser.FirstName = user.FirstName;
                    getUser.LastName = user.LastName;
                    getUser.Email = user.Email;
                    getUser.PhoneNumber = user.PhoneNumber;
                    getUser.Password = user.PasswordHash;
                }
                else
                {
                    return Unauthorized(new { message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Invalid token", details = ex.Message });
            }
            return Ok(getUser);
        }


        [HttpPost]
        [Authorize]
        public IActionResult PostNewData(GetProfile profile)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var userId = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        return Unauthorized(new { message = "User ID not found in token." });
                    }
                    User user = _context.Users.FirstOrDefault(x => x.Id == userId);
                    if (user != null)
                    {
                        user.FirstName = profile.FirstName;
                        user.LastName = profile.LastName;
                        user.Email = profile.Email;
                        user.PhoneNumber = profile.PhoneNumber;
                        user.PasswordHash = profile.Password;

                        _context.Users.Update(user);
                        _context.SaveChanges();

                        return Ok();
                    }
                    else
                    {
                        return Unauthorized(new { message = "User not found" });
                    }
                }
                catch (Exception ex)
                {
                    return Unauthorized(new { message = "Invalid token", details = ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
