using PokemonReviewApp.Models.ModelsForJwt;

namespace PokemonReviewApp.Interfaces.JWT
{
    public interface IJwtService
    {
        public string Generate(UserModel user);
        public UserModel Authenticate(UserLogin userLogin);
    }
}
