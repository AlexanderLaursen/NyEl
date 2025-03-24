using API.Repositories.Interfaces;
using Common.Dtos.BillingModel;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillingModelsController : ControllerBase
    {
        private readonly ICommonRepository<BillingModel> _billingModelRepository;
        private readonly ILogger<BillingModelsController> _logger;

        public BillingModelsController(ICommonRepository<BillingModel> billingModelRepository, ILogger<BillingModelsController> logger)
        {
            _billingModelRepository = billingModelRepository;
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
                var billingModel = await _billingModelRepository.GetByIdAsync(id);

                if (billingModel == null)
                {
                    _logger.LogWarning($"BillingModel with ID {id} not found.");
                    return NotFound();
                }

                BillingModelDto billingModelDto = billingModel.Adapt<BillingModelDto>();
                return Ok(billingModelDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving BillingModel with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving BillingModel with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
