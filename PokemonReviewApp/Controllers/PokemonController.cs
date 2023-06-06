using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository,
                IOwnerRepository ownerRepository,
                IReviewRepository reviewRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of pokemons.
        /// </summary>
        /// <response code="200">c</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        /// <summary>
        /// Gets pokemon by it's ID.
        /// </summary>
        /// <param name="pokemonId">ID of the pokemon to get.</param>
        /// <response code="200">Returns the pokemon with the specified ID.</response>>
        /// <response code="400">If the request is invalid.</response>>
        [HttpGet("{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(pokemonId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        /// <summary>
        /// Gets the rating of the pokemon.
        /// </summary>
        /// <param name="pokemonId">ID of the pokemon want to get rating.</param>
        /// <response code="200">Returns the pokemon's rating.</response>>
        /// <response code="400">If the request is invalid.</response>>
        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var rating = _pokemonRepository.GetPokemonRating(pokemonId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(rating);
        }

        /// <summary>
        /// Creates a new pokemon.
        /// </summary>
        /// <param name="ownerId">ID of the owner.</param>
        /// <param name="categoryId">ID of the category.</param>
        /// <param name="pokemonCreate">The pokemon to be created.</param>
        /// <response code="204">If a pokemon created successfully.</response>
        /// <response code="400">If the request is invalid.</response>        
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemonCreate)
        {
            if (pokemonCreate == null)
            {
                return BadRequest(ModelState);
            }

            var pokemons = _pokemonRepository.GetPokemonTrimUpper(pokemonCreate);

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Updates a pokemon with the specified ID.
        /// </summary>
        /// <param name="pokemonId">The ID of the pokemon to be updated.</param>
        /// <param name="ownerId">The ID of the owner to be updated.</param>
        /// <param name="categoryId">The ID of the category to be updated.</param>
        /// <param name="updatedPokemon">The updated pokemon data.</param>
        /// <response code="204">If the pokemon is successfully updated.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If a pokemon with the specified ID is not found.</response>
        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId,
                [FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO updatedPokemon)
        {
            if (updatedPokemon == null)
            {
                return BadRequest(ModelState);
            }

            if (!pokemonId.Equals(updatedPokemon.Id))
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Removes a pokemon with the specified ID.
        /// </summary>
        /// <param name="pokemonId">The ID of the pokemon to be removed.</param>
        /// <response code="204">If the pokemon is successfully removed.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If a pokemon with the specified ID is not found.</response>
        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewsOfAPokemon(pokemonId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviews");
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting pokemon");
            }

            return NoContent();
        }
    }
}
