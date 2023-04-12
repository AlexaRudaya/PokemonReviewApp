using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository,
                ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of owners.
        /// </summary>
        /// <response code="200">Returns all of the owners.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }

        /// <summary>
        /// Gets owner by his ID.
        /// </summary>
        /// <param name="ownerId">ID of the owner to get.</param>
        /// <response code="200">Returns the owner with the specified ID.</response>>
        /// <response code="400">If the request is invalid.</response>>
        [HttpGet("ownerId")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        /// <summary>
        /// Gets a pokemon by his owner.
        /// </summary>
        /// <param name="ownerId">ID of the owner.</param>
        /// <response code="200">Returns the pokemon with the specified owner.</response>>
        /// <response code="400">If the request is invalid.</response>>
        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<List<PokemonDTO>>(_ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            return Ok(owner);
        }

        /// <summary>
        /// Creates a new owner.
        /// </summary>
        /// <param name="countryId">ID of the country.</param>
        /// <param name="ownerCreate">The owner to be created.</param>
        /// <response code="204">If an owner created successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var owner = _ownerRepository.GetOwners()
                .Where(_ => _.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        ///  Updates an owner with the specified ID.
        /// </summary>
        /// <param name="ownerId">The ID of the owner to be updated.</param>
        /// <param name="updatedOwner">The updated owner data.</param>
        /// <response code="204">If the owner is successfully updated.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If an owner with the specified ID is not found.</response>
        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody]OwnerDTO updatedOwner)
        {
            if (updatedOwner == null)
            { 
                return BadRequest(ModelState);
            }

            if (!ownerId.Equals(updatedOwner.Id))
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Removes an owner with the specified ID.
        /// </summary>
        /// <param name="ownerId">The ID of the owner to be removed.</param>
        /// <response code="204">If the owner is successfully removed.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If an owner with the specified ID is not found.</response>
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId)) 
            {
                return NotFound();
            }

            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}
