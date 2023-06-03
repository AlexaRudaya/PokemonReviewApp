using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Interfaces.JWT;
using PokemonReviewApp.Models.ModelsForJwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokemonReviewApp.Services
{
    public sealed class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8
                                                                .GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                new Claim(ClaimTypes.Email, user.EmailAddress!),
                new Claim(ClaimTypes.GivenName, user.GivenName!),
                new Claim(ClaimTypes.Surname, user.Surname!),
                new Claim(ClaimTypes.Role, user.Role!),
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                               .WriteToken(token);
        }

        public UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = JwtSeedData.Users
                                         .FirstOrDefault(_ => _.UserName!.ToLower() == userLogin.UserName!.ToLower()
                                         &&
                                         _.Password == userLogin.Password);
            if (currentUser is not null)
            {
                return currentUser;
            }

            return null;
        }
    }
}
