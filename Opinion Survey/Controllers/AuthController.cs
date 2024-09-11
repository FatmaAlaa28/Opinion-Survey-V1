using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Opinion_Survey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {
            var redirectUrl = Url.Action(nameof(HandleGoogleResponse), "Auth", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("handle-google-response")]
        public async Task<IActionResult> HandleGoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                var claims = result.Principal.Claims;
                // Process claims as needed
                return Ok(new { Message = "Login successful", Claims = claims });
            }

            return Unauthorized();
        }
    }

}
