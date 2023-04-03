using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository; 
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of all categories.
        /// </summary>
        /// <returns>The list of the categories.</returns>
        /// <response code="200">Returns all of the categories.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        /// <summary>
        /// Gets category by it's ID.
        /// </summary>
        /// <param name="categoryId">ID of the category we want to get.</param>
        /// <returns>The category with the specified ID.</returns>
        /// <response code="200">Returns the category with the specified ID.</response>
        /// <response code="400">If the request is invalid or missing required fields.</response>
        [HttpGet("categoryId")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId)) 
            {
                return NotFound();
            }

            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);
        }

        /// <summary>
        /// Gets Pokemon by it's category.
        /// </summary>
        /// <param name="categoryId">ID of the pokemon's category.</param>
        /// <returns>The list of pokemons with certain category.</returns>
        /// <response code="200">Returns pokemons with certain category.</response>
        /// <response code="400">If the request is invalid or missing required fields.</response>
        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategory(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(
                    _categoryRepository.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(pokemons);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryCreate">The category to create.</param>
        /// <returns>Returns an HTTP 204 No Content status code if the category is created successfully, 
        /// or an HTTP 400 Bad Request status code if the request is invalid or missing required fields.</returns>
        /// <response code="204">The category was created successfully.</response>
        /// <response code="400">If the request is invalid or missing required fields.</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
        {
            if (categoryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var category = _categoryRepository.GetCategories()
                    .Where(_ => _.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                    .FirstOrDefault();

            if (category != null) 
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap)) 
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryId">ID of the category to update.</param>
        /// <param name="updatedCategory">The updated category information.</param>
        /// <returns>Returns an HTTP 204 No Content status code if the category is updated successfully, 
        /// an HTTP 400 Bad Request status code if the request is invalid or missing required fields 
        /// or an HTTP 404 Not Found status code if the category to update can't be found.</returns>
        /// <response code="400">If the request is invalid or missing required fields.</response>
        /// <response code="204">The category was updated successfully.</response>
        /// <response code="404">If the category to update can't be found.</response>
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDTO updatedCategory)
        {
            if (updatedCategory == null)
            {
                return BadRequest(ModelState);
            }

            if (!categoryId.Equals(updatedCategory.Id))
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState); 
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing category.
        /// </summary>
        /// <param name="categoryId">ID of the category to delete.</param>
        /// <returns>Returns an HTTP 204 No Content status code if the category is deleted successfully, 
        /// an HTTP 400 Bad Request status code if the request is invalid or missing required fields
        /// or an HTTP 404 Not Found status code if the category to delete can't be found.</returns>
        /// <response code="400">If the request is invalid or missing required fields.</response>
        /// <response code="204">The category was deleted successfully.</response>
        /// <response code="404">If the category to delet can't be found.</response>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
