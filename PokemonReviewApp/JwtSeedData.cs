using PokemonReviewApp.Models.ModelsForJwt;

namespace PokemonReviewApp
{
    public sealed class JwtSeedData
    {
        public static List<UserModel> Users = new()
        {
            new UserModel() {UserName = "John_admin", EmailAddress = "admin.john@gmail.com" ,
                             Password = "Admin_Password7", GivenName = "John", Surname = "Smith", 
                             Role = "Administrator"},

            new UserModel() {UserName = "Kate_user", EmailAddress = "user.kate@gmail.com",
                             Password = "User_Password8", GivenName = "Kate", Surname = "Clark",
                             Role = "User"}
        };
    }
}
