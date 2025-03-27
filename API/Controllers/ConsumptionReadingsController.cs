using System.Security.Claims;
using API.Repositories.Interfaces;
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
    public class ConsumptionReadingsController : ControllerBase
    {
        private readonly ICommonRepository<ConsumptionReading> _commonRepository;
        private readonly ILogger<ConsumptionReadingsController> _logger;
        private readonly IConsumptionService _consumptionService;

        public ConsumptionReadingsController(ICommonRepository<ConsumptionReading> commonRepository, ILogger<ConsumptionReadingsController> logger,
            IConsumptionService consumptionService)
        {
            _commonRepository = commonRepository;
            _logger = logger;
            _consumptionService = consumptionService;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetByTimeframe(DateTime startDate, TimeframeOptions timeframeOptions)
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized();
                }

                ConsumptionReadingListDto result = await _consumptionService.GetConsumptionReadingsAsync(startDate, timeframeOptions, userId);

                if (result == null)
                {
                    _logger.LogWarning($"No consumption readings found for user {userId} in the specified timeframe.");
                    return NotFound();
                }

                return Ok(result);
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
