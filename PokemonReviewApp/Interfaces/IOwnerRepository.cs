using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        public ICollection<Owner> GetOwners();

        public Owner GetOwner(int ownerId);

        public ICollection<Owner> GetOwnerOfAPokemon(int pokemonId);

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId);

        public bool OwnerExists(int ownerId);

        public bool CreateOwner(Owner owner);

        public bool UpdateOwner(Owner owner);
        public bool Save();
    }
}
