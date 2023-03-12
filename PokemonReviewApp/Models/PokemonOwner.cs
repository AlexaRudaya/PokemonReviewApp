namespace PokemonReviewApp.Models
{
    public sealed class PokemonOwner
    {
        public int PokemonId { get; set; }

        public int OwnerId { get; set; }

        public Pokemon? Pokemon { get; set; }

        public Owner? Owner { get; set; }
    }
}
