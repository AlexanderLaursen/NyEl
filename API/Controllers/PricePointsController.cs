using API.Repositories.Interfaces;
using Common.Dtos.PricePoint;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PricePointsController : ControllerBase
    {
        private readonly ICommonRepository<PricePoint> _pricePointRepository;
        private readonly ILogger<PricePointsController> _logger;

        public PricePointsController(ICommonRepository<PricePoint> pricePointRepository, ILogger<PricePointsController> logger)
        {
            _pricePointRepository = pricePointRepository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Invalid ID provided: {id}");
                return BadRequest("The ID must be a positive integer.");
            }

            try
            {
                var pricePoint = await _pricePointRepository.GetByIdAsync(id);

                if (pricePoint == null)
                {
                    _logger.LogWarning($"PricePoint with ID {id} not found.");
                    return NotFound();
                }

                PricePointDto pricePointDto = pricePoint.Adapt<PricePointDto>();
                return Ok(pricePointDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving PricePoint with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving PricePoint with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePricePointDto createPricePointDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a PricePoint.");
                return BadRequest(ModelState);
            }

            try
            {
                var pricePoint = createPricePointDto.Adapt<PricePoint>();
                await _pricePointRepository.AddAsync(pricePoint);
                return CreatedAtAction(nameof(Post), pricePoint.Adapt<PricePointDto>());
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new PricePoint.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a new PricePoint.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
