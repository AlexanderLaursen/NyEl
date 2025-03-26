using System.Security.Claims;
using API.Models.TimeframeStrategy;
using API.Repositories.Interfaces;
using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/consumption-readings")]
    public class ConsumptionReadingsController : ControllerBase
    {
        private readonly ICommonRepository<ConsumptionReading> _commonRepository;
        private readonly ILogger<ConsumptionReadingsController> _logger;
        private readonly IConsumptionRepository _consumptionRepository;

        public ConsumptionReadingsController(ICommonRepository<ConsumptionReading> commonRepository, ILogger<ConsumptionReadingsController> logger,
            IConsumptionRepository consumptionRepository)
        {
            _commonRepository = commonRepository;
            _logger = logger;
            _consumptionRepository = consumptionRepository;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetByTimeframe(DateTime startDate, TimeframeOptions timeframeOptions)
        {
            try
            {
                TimeframeContext timeframeContext = new(timeframeOptions);
                Timeframe timeframe = timeframeContext.GetTimeframe(startDate);

                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized();
                }

                var result = await _consumptionRepository.GetConsumptionReadingsAsync(userId, timeframe);

                if (result == null)
                {
                    _logger.LogWarning($"No consumption readings found for user {userId} in the specified timeframe.");
                    return NotFound();
                }

                ConsumptionReadingListDto consumptionReadingListDto = new()
                {
                    ConsumptionReadings = result.Adapt<IEnumerable<ConsumptionReadingDto>>()
                };

                return Ok(consumptionReadingListDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving consumption readings.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving consumption readings.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
                var consumptionReading = await _commonRepository.GetByIdAsync(id);

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
                await _commonRepository.AddAsync(consumptionReading);
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
