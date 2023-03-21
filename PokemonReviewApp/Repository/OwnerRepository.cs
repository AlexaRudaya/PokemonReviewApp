using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public sealed class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public Owner? GetOwner(int ownerId)
        {
            return _context.Owners
                        .Where(_ => _.Id == ownerId)
                        .FirstOrDefault();
        }

        public ICollection<Owner?> GetOwnerOfAPokemon(int pokemonId)
        {
            return _context.PokemonOwners
                        .Where(_ => _.Pokemon.Id == pokemonId)
                        .Select(_ => _.Owner)
                        .ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon?> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners
                        .Where(_=> _.Owner.Id == ownerId)
                        .Select(_ => _.Pokemon)
                        .ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners
                        .Any(_ => _.Id == ownerId);
        }
    }
}
