using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Opinion_Survey.DTO;
using Opinion_Survey.Models;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _hosting;
        public ProfileController(AppDbContext context, UserManager<User> userManager,
            IHostingEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;

        }
        [HttpGet("GetUpdateProfile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            ProfileDTO getUser = new();
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
                    getUser.Imagepath = user.Imagepath;
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


        [HttpPost("postUpdateProfile")]
        [Authorize]
        public async Task<IActionResult> PostNewData([FromForm] ProfileDTO profile)
        {
            if (ModelState.IsValid)
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

                        // Handle image file upload
                        if (profile.ImageFile != null && profile.ImageFile.FileName != "Default.jpeg")
                        {
                            string ImageFolderPath = Path.Combine(_hosting.WebRootPath, "Images");
                            string NewImagePath = Path.Combine(ImageFolderPath, profile.ImageFile.FileName);
                            using (var stream = new FileStream(NewImagePath, FileMode.Create))
                            {
                                await profile.ImageFile.CopyToAsync(stream);
                            }
                            user.Imagepath = profile.ImageFile.FileName;
                        }
                        else
                        {
                            user.Imagepath = profile.Imagepath;
                        }

                        // Handle password change
                        if (!string.IsNullOrEmpty(profile.CurrentPassword) && !string.IsNullOrEmpty(profile.NewPassword))
                        {
                            var result = await _userManager.ChangePasswordAsync(user, profile.CurrentPassword, profile.NewPassword);
                            if (!result.Succeeded)
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                                return BadRequest(ModelState);
                            }
                        }

                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();
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
