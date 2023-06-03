using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces.JWT;
using PokemonReviewApp.Models.ModelsForJwt;

namespace PokemonReviewApp.Controllers.JWT_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public LoginController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _jwtService.Authenticate(userLogin);

            if (user is not null) 
            {
                var token = _jwtService.Generate(user);
                return Ok(token);
            }

            return NotFound("User was not found");          
        }
    }
}
