using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the list of reviewers.
        /// </summary>
        /// <response code="200">Returns all of the reviewers.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewers);
        }

        /// <summary>
        /// Gets reviewer by his ID.
        /// </summary>
        /// <param name="reviewerId">ID of the reviewer to get.</param>
        /// <response code="200">Returns the reviewer with the specified ID.</response>>
        /// <response code="400">If the request is invalid.</response>>
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId) 
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewer = _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewer);
        }

        /// <summary>
        /// Gets reviews of a certain reviewer.
        /// </summary>
        /// <param name="reviewerId">ID of the reviewer who's reviewes want to get.</param>
        /// <response code="200">Returns the reviewer's reviews.</response>>
        /// <response code="400">If the request is invalid.</response>>
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewerRepository.GetReviewsByAReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        /// <summary>
        /// Creates a new reviewer.
        /// </summary>
        /// <param name="reviewerCreate">The reviewer to be created.</param>
        /// <response code="204">If a reviewer created successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerCreate)
        {
            if (reviewerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var reviewer = _reviewerRepository.GetReviewers()
                                .Where(_ => _.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
                                .FirstOrDefault();

            if (reviewer != null) 
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Updates a reviewer with the specified ID.
        /// </summary>
        /// <param name="reviewerId">The ID of the reviewer to be updated.</param>
        /// <param name="updatedReviewer">The updated reviewer data.</param>
        /// <response code="204">If the reviewer is successfully updated.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If a reviewer with the specified ID is not found.</response>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO updatedReviewer)
        {
            if (updatedReviewer == null)
            {
                return BadRequest(ModelState);
            }

            if (!reviewerId.Equals(updatedReviewer.Id))
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId)) 
            {
                return NotFound();
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Removes a reviewer with the specified ID.
        /// </summary>
        /// <param name="reviewerId">The ID of the reviewer to be removed.</param>
        /// <response code="204">If the reviewer is successfully removed.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If a reviewer with the specified ID is not found.</response>
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId) 
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
            }

            return NoContent();
        }
    }
}
