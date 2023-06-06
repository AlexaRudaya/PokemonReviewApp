namespace PokemonReviewApp.Tests.Repositories
{
    public class PokemonRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                                       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                       .Options;

            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Pokemon.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Pokemon.Add(
                        new Pokemon()
                        {
                            Name = "Pikachu",
                            BirthDate = new DateTime(2002, 1, 2),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Electric"} }
                            },

                            Reviews = new List<Review>()
                            {
                                new Review { Title = "Pikachu", Text = "Pikachuuuu", Rating = 5,
                                Reviewer = new Reviewer() { FirstName = "John", LastName = "Smith"} },

                                new Review { Title = "Pikachu2", Text = "Pikachuuuu new one", Rating = 4,
                                Reviewer = new Reviewer() { FirstName = "Kate", LastName = "Lamy" } },

                                new Review { Title = "Pikachu3", Text = "Pikachuuuu, chu-chu", Rating = 3 ,
                                Reviewer = new Reviewer() { FirstName = "Liam", LastName = "Johens" } }
                            }
                        });

                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async Task PokemonRepository_GetPokemon_ReturnsPokemon()
        {
            //Arrange
            var name = "Pikachu";
            var dbContext = await GetDatabaseContext();
            var pokemonRepository = new PokemonRepository(dbContext);
            
            //Act
            var result = pokemonRepository.GetPokemon(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Pokemon>();
        }

        [Fact]
        public async Task PokemonRepository_GetPokemonRating_ReturnsDecimalBetween1and10()
        {
            //Arrange
            var pokemonId = 1;
            var dbContext = await GetDatabaseContext();
            var pokemonRepository = new PokemonRepository(dbContext);

            //Act
            var result = pokemonRepository.GetPokemonRating(pokemonId);

            //Assert
            result.Should().NotBe(0);
            result.Should().BeInRange(1, 10);
        }
    }
}
