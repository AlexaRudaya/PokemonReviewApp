using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        public ICollection<Review> GerReviews();

        public Review GetReview(int reviewId);

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId);

        public bool ReviewExists(int reviewId);
    }
}
