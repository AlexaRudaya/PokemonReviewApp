using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Models.ModelsForJwt;
using System.Security.Claims;

namespace PokemonReviewApp.Controllers.JWT_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Admins")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Admins()
        { 
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are an {currentUser.Role}");
        }

        [HttpGet("Users")]
        [Authorize(Roles = "User")]
        public IActionResult Users()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are an {currentUser.Role}");
        }
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    UserName = userClaims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(_ => _.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(_ => _.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(_ => _.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(_ => _.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }
    }
}

