using API.Repositories.Interfaces;
using Common.Dtos.PriceInfo;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/price-info")]
    public class PriceInfoController : ControllerBase
    {
        private readonly ICommonRepository<PriceInfo> _priceInfoRepository;
        private readonly ILogger<PriceInfoController> _logger;

        public PriceInfoController(ICommonRepository<PriceInfo> priceInfoRepository, ILogger<PriceInfoController> logger)
        {
            _priceInfoRepository = priceInfoRepository;
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
                var priceInfo = await _priceInfoRepository.GetByIdAsync(id);

                if (priceInfo == null)
                {
                    _logger.LogWarning($"PriceInfo with ID {id} not found.");
                    return NotFound();
                }

                PriceInfoDto priceInfoDto = priceInfo.Adapt<PriceInfoDto>();
                return Ok(priceInfoDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving PriceInfo with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving PriceInfo with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePriceInfoDto createPriceInfoDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a PriceInfo.");
                return BadRequest(ModelState);
            }

            try
            {
                var priceInfo = createPriceInfoDto.Adapt<PriceInfo>();
                await _priceInfoRepository.AddAsync(priceInfo);
                return CreatedAtAction(nameof(Post), priceInfo.Adapt<PriceInfoDto>());
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new PriceInfo.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a new PriceInfo.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
