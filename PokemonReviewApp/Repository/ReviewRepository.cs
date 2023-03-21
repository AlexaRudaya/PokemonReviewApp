using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public sealed class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context) 
        {
            _context = context;
        }

        public ICollection<Review> GerReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review? GetReview(int reviewId)
        {
            return _context.Reviews
                        .Where(_ => _.Id == reviewId)
                        .FirstOrDefault();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
        {
            return _context.Reviews
                        .Where(_ => _.Pokemon.Id == pokemonId)
                        .ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews
                        .Any(_ => _.Id == reviewId);
        }
    }
}
