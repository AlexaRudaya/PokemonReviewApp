using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public sealed class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public Pokemon? GetPokemon(int id)  // it's gonna be a detail page
        {
            return _context.Pokemon
                        .Where(_ => _.Id == id)
                        .FirstOrDefault();
        }

        public Pokemon? GetPokemon(string name)
        {
            return _context.Pokemon
                        .Where(_ => _.Name == name)
                        .FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokemonId)
        {
            var review = _context.Reviews
                            .Where(_ => _.Pokemon!.Id == pokemonId);

            if (review.Count() <= 0)
            {
                return 0;
            }
            else
            {
                return ((decimal)review.Sum(_ => _.Rating) / review.Count());   
            }
        } 

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon
                        .OrderBy(_ => _.Id).ToList(); // ToList - crucial.
        }

        public bool PokemonExists(int pokemonId)
        {
            return _context.Pokemon
                        .Any(_ => _.Id == pokemonId);
        }
    }
}
