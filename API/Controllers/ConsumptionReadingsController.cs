using System.Security.Claims;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
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
    public class ConsumptionReadingsController : BaseController
    {
        private readonly ICommonRepository<ConsumptionReading> _commonRepository;
        private readonly ILogger<ConsumptionReadingsController> _logger;
        private readonly IConsumptionService _consumptionService;

        public ConsumptionReadingsController(
            IConsumptionService consumptionService,
            IConsumerService consumerService,
            ICommonRepository<ConsumptionReading> commonRepository,
            ILogger<ConsumptionReadingsController> logger)
            : base(consumerService)
        {
            _commonRepository = commonRepository;
            _logger = logger;
            _consumptionService = consumptionService;
        }

        // Return consumption readings for a specific consumer in a specific timeframe
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByTimeframe(DateTime startDate, TimeframeOptions timeframeOptions)
        {
            try
            {
                int consumerId = await GetConsumerId();
                ConsumptionReadingListDto result = await _consumptionService.GetConsumptionReadingsAsync(startDate, timeframeOptions, consumerId);

                if (result == null)
                {
                    _logger.LogWarning($"No consumption readings found for user {consumerId} in the specified timeframe.");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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

        // Post a new consumption for the provided user id
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
                ConsumptionReading consumptionReading = createConsumptionReadingDto.Adapt<ConsumptionReading>();
                await _consumptionService.AddAsync(consumptionReading);

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

        // Post a range of consumption readings
        [HttpPost("range")]
        public async Task<IActionResult> PostRange(CreateConsumptionReadingListDto createConsumptionReadingListDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a ConsumptionReading.");
                return BadRequest(ModelState);
            }

            try
            {
                List<ConsumptionReading> consumptionReadings = createConsumptionReadingListDto
                    .CreateConsumptionReadingDtos.Adapt<List<ConsumptionReading>>();

                await _consumptionService.AddRangeAsync(consumptionReadings);
                return CreatedAtAction(nameof(Post), consumptionReadings.Adapt<List<ConsumptionReadingDto>>());
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
