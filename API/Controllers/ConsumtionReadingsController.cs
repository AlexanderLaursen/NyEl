using API.Repositories.Interfaces;
using Common.Dtos.ConsumptionReading;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumptionReadingsController : ControllerBase
    {
        private readonly ICommonRepository<ConsumptionReading> _consumptionReadingRepository;
        private readonly ILogger<ConsumptionReadingsController> _logger;

        public ConsumptionReadingsController(ICommonRepository<ConsumptionReading> consumptionReadingRepository, ILogger<ConsumptionReadingsController> logger)
        {
            _consumptionReadingRepository = consumptionReadingRepository;
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
                var consumptionReading = await _consumptionReadingRepository.GetByIdAsync(id);

                if (consumptionReading == null)
                {
                    _logger.LogWarning($"ConsumptionReading with ID {id} not found.");
                    return NotFound();
                }

                ConsumptionReadingDto consumptionReadingDto = consumptionReading.Adapt<ConsumptionReadingDto>();
                return Ok(consumptionReadingDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving ConsumptionReading with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving ConsumptionReading with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateConsumptionReadingDto createConsumptionReadingDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a ConsumptionReading.");
                return BadRequest(ModelState);
            }

            try
            {
                var consumptionReading = createConsumptionReadingDto.Adapt<ConsumptionReading>();
                await _consumptionReadingRepository.AddAsync(consumptionReading);
                return CreatedAtAction(nameof(Post), consumptionReading.Adapt<ConsumptionReadingDto>());
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new ConsumptionReading.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a new ConsumptionReading.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
