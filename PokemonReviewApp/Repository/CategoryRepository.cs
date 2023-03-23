using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories
                        .Any(_ => _.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category? GetCategory(int id)
        {
            return _context.Categories
                        .Where(_ => _.Id == id)
                        .FirstOrDefault();
        }

        public ICollection<Pokemon?> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories
                        .Where(_ => _.CategoryId == categoryId)
                        .Select(_ => _.Pokemon) 
                        .ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false; 
        }
    }
}
