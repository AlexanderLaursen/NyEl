using API.Repositories.Interfaces;
using Common.Dtos.PriceInfo;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using API.Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/price-info")]
    public class PriceInfoController : Controller
    {
        private readonly ICommonRepository<PriceInfo> _commonRepository;
        private readonly IPriceInfoService _priceInfoService;
        private readonly ILogger<PriceInfoController> _logger;

        public PriceInfoController(ICommonRepository<PriceInfo> commonRepository, ILogger<PriceInfoController> logger,
            IPriceInfoService priceInfoService)
        {
            _commonRepository = commonRepository;
            _priceInfoService = priceInfoService;
            _logger = logger;
        }

        // Post new price info
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
                await _commonRepository.AddAsync(priceInfo);
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

        // Get priceinfo by timeframe
        [HttpGet()]
        public async Task<IActionResult> GetByTimeframe(DateTime startDate, TimeframeOptions timeframeOptions)
        {
            try
            {
                var result = await _priceInfoService.GetPriceInfoAsync(startDate, timeframeOptions);

                if (result == null)
                {
                    _logger.LogWarning($"No price info found for the specified timeframe.");
                    return NotFound();
                }

                PriceInfoListDto priceInfoListDto = new()
                {
                    PriceInfoList = result.Adapt<List<PriceInfoDto>>()
                };

                return Ok(priceInfoListDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving price info.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving price info.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
