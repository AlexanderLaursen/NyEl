using System.Security.Claims;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Dtos.BillingModel;
using Common.Dtos.Consumer;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/consumers")]
    public class ConsumersController : ControllerBase
    {
        private readonly IConsumerRepository _consumerRepository;
        private readonly IConsumerService _consumerService;
        private readonly IInvoicePreferenceRepository _invoicePreferenceRepository;
        private readonly ILogger<ConsumersController> _logger;

        public ConsumersController(IConsumerRepository consumerRepository, ILogger<ConsumersController> logger,
            IInvoicePreferenceRepository invoicePreferenceRepository, IConsumerService consumerService)
        {
            _consumerRepository = consumerRepository;
            _logger = logger;
            _invoicePreferenceRepository = invoicePreferenceRepository;
            _consumerService = consumerService;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetByUserClaims()
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int consumerId = await _consumerService.GetConsumerId(userId);

                Consumer consumer = await _consumerRepository.GetConsumerByUserIdAsync(consumerId);

                if (consumer == null)
                {
                    _logger.LogWarning($"Consumer with ID {userId} not found.");
                    return NotFound();
                }

                ConsumerDto consumerDto = new()
                {
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    PhoneNumber = consumer.PhoneNumber,
                    Email = consumer.Email,
                    CPR = consumer.CPR,
                    UserId = consumer.UserId,
                    BillingModel = consumer.BillingModel.BillingModelType
                };

                return Ok(consumerDto);
            }
            catch (UnkownUserException ex)
            {
                _logger.LogWarning(ex, $"User not found.");
                return Unauthorized();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving Consumer.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving Consumer.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpGet("full")]
        public async Task<IActionResult> GetByUserClaimsFull()
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int consumerId = await _consumerService.GetConsumerId(userId);

                Consumer consumer = await _consumerRepository.GetConsumerByUserIdAsync(consumerId);
                List<InvoicePreference> invoicePreferences = await _invoicePreferenceRepository.GetByConsumerIdAsync(consumerId);

                if (consumer == null)
                {
                    _logger.LogWarning($"Consumer with ID {userId} not found.");
                    return NotFound();
                }

                ConsumerDtoFull consumerDto = new()
                {
                    Id = consumer.Id,
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    PhoneNumber = consumer.PhoneNumber,
                    Email = consumer.Email,
                    CPR = consumer.CPR,
                    BillingModel = consumer.BillingModel.BillingModelType,
                    InvoicePreferences = invoicePreferences.Select(ip => ip.InvoicePreferenceType).ToList()
                };

                return Ok(consumerDto);
            }
            catch (UnkownUserException ex)
            {
                _logger.LogWarning(ex, $"User not found.");
                return Unauthorized();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving Consumer with");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving Consumer with");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateConsumerDto createConsumerDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a Consumer.");
                return BadRequest(ModelState);
            }

            try
            {
                var consumer = createConsumerDto.Adapt<Consumer>();
                await _consumerRepository.AddConsumerAsync(consumer);
                return CreatedAtAction(nameof(Post), consumer.Adapt<ConsumerDto>());
            }
            catch (UnkownUserException ex)
            {
                _logger.LogWarning(ex, $"User not found.");
                return Unauthorized();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new Consumer.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a new Consumer.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpPost("update-billing")]
        public async Task<IActionResult> UpdateBilling(BillingModelDto billingModelDto)
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int consumerId = await _consumerService.GetConsumerId(userId);

                int result = await _consumerRepository.UpdateBillingModelAsync(billingModelDto.BillingModelType, consumerId);

                if (result == 0)
                {
                    _logger.LogWarning($"Consumer with ID {consumerId} not found.");
                    return NotFound();
                }

                return NoContent();
            }
            catch(UnkownUserException ex)
            {
                _logger.LogWarning(ex, $"User not found.");
                return Unauthorized();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error updating Billing Model for Consumer.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error updating Billing Model for Consumer.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}