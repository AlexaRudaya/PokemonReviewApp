namespace PokemonReviewApp.Models
{
    public sealed class Country
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public ICollection<Owner>? Owners { get; set; }
    }
}
