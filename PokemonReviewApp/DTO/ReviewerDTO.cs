using PokemonReviewApp.Models;

namespace PokemonReviewApp.DTO
{
    public sealed class ReviewerDTO
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
