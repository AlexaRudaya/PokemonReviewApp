using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of all countries.
        /// </summary>
        /// <returns>The list of all countries.</returns>
        /// <response code="200">Returns all of the countries.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }

        /// <summary>
        /// Gets country by it's ID.
        /// </summary>
        /// <param name="countryId">ID of the country to get.</param>
        /// <returns>The country with the specified ID.</returns>
        /// <response code="200">Returns the country with the specified ID.</response>>
        /// <response code="400">If the request is invalid.</response>>
        [HttpGet("countryId")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        /// <summary>
        /// Gets the country of an owner by owner ID.
        /// </summary>
        /// <param name="ownerId">The ID of the owner.</param>
        /// <response code="200">Returns the country of the owner.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDTO>(
                    _countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        /// <summary>
        /// Creates a new country.
        /// </summary>
        /// <param name="countryCreate">The country to be created.</param>
        /// <response code="204">If a country created successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries()
                .Where(_ => _.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Updates a country with the specified ID.
        /// </summary>
        /// <param name="countryId">The ID of the country to be updated.</param>
        /// <param name="updatedCountry">The updated country data.</param>
        /// <response code="204">If the country is successfully updated.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If a country with the specified ID is not found.</response>
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDTO updatedCountry)
        {
            if (updatedCountry == null)
            {
                return BadRequest(ModelState);
            }

            if (!countryId.Equals(updatedCountry.Id))
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Removes a country with the specified ID.
        /// </summary>
        /// <param name="countryId">The ID of the country to be removed.</param>
        /// <response code="204">If the country is successfully removed.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If a country with the specified ID is not found.</response>
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting country");
            }

            return NoContent();
        }
    }
}
