using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        public ICollection<Review> GetReviews();

        public Review GetReview(int reviewId);

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId);

        public bool ReviewExists(int reviewId);

        public bool CreateReview(Review review);

        public bool UpdateReview(Review review);

        public bool Save();
    }
}
