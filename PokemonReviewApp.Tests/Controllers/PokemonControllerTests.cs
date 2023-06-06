namespace PokemonReviewApp.Tests.Controllers
{
    public class PokemonControllerTests
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public PokemonControllerTests()
        {
            _pokemonRepository = A.Fake<IPokemonRepository>();
            _reviewRepository = A.Fake<IReviewRepository>();
            _ownerRepository = A.Fake<IOwnerRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void PokemonController_GetPokemons_ReturnsOk()
        {
            //Arrange
            var pokemons = A.Fake<ICollection<PokemonDTO>>();
            var pokemonList = A.Fake<List<PokemonDTO>>();

            A.CallTo(() => _mapper.Map<List<PokemonDTO>>(pokemons))
                                                        .Returns(pokemonList);

            var controller = new PokemonController(_pokemonRepository, _ownerRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.GetPokemons();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void PokemonController_CreatePokemon_ReturnsOk()
        {
            //Arrange
            int ownerId = 1;
            int categoryId = 2;
            var pokemonMap = A.Fake<Pokemon>();
            var pokemon = A.Fake<Pokemon>();
            var pokemonCreate = A.Fake<PokemonDTO>();
            var pokemons = A.Fake<ICollection<PokemonDTO>>();
            var pokemonList = A.Fake <IList<PokemonDTO>>();

            A.CallTo(() => _pokemonRepository.GetPokemonTrimUpper(pokemonCreate))
                                                                  .Returns(pokemon);
                               
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonCreate))
                                               .Returns(pokemon);

            A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon))
                                                            .Returns(true);

            var controller = new PokemonController(_pokemonRepository, _ownerRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.CreatePokemon(ownerId, categoryId, pokemonCreate);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
