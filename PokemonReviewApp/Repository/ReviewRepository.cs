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

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public ICollection<Review> GetReviews()
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

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
