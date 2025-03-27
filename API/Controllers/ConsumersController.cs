using System.Security.Claims;
using API.Repositories.Interfaces;
using Common.Dtos.BillingModel;
using Common.Dtos.Consumer;
using Common.Enums;
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
        private readonly IInvoicePreferenceRepository _invoicePreferenceRepository;
        private readonly ILogger<ConsumersController> _logger;

        public ConsumersController(IConsumerRepository consumerRepository, ILogger<ConsumersController> logger,
            IInvoicePreferenceRepository invoicePreferenceRepository)
        {
            _consumerRepository = consumerRepository;
            _logger = logger;
            _invoicePreferenceRepository = invoicePreferenceRepository;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetByUserClaims()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }
            try
            {
                Consumer consumer = await _consumerRepository.GetByUserIdAsync(userId);

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
                    BillingModel = consumer.BillingModel.BillingModelMethod
                };

                return Ok(consumerDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving Consumer with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving Consumer with ID {userId}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpGet("full")]
        public async Task<IActionResult> GetByUserClaimsFull()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }
            try
            {
                Consumer consumer = await _consumerRepository.GetByUserIdAsync(userId);
                List<InvoicePreference> invoicePreferences = await _invoicePreferenceRepository.GetByUserIdAsync(userId);

                if (consumer == null)
                {
                    _logger.LogWarning($"Consumer with ID {userId} not found.");
                    return NotFound();
                }

                ConsumerDtoFull consumerDto = new()
                {
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    PhoneNumber = consumer.PhoneNumber,
                    Email = consumer.Email,
                    CPR = consumer.CPR,
                    UserId = consumer.UserId,
                    BillingModel = consumer.BillingModel.BillingModelMethod,
                    InvoicePreferences = invoicePreferences.Select(ip => ip.InvoiceNotificationPreference).ToList()
                };

                return Ok(consumerDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving Consumer with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving Consumer with ID {userId}.");
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
        [HttpPut]
        public async Task<IActionResult> Update(BillingModelDto billingModelDto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                int result = await _consumerRepository.UpdateBillingModelAsync(billingModelDto, userId);

                if (result == 0)
                {
                    _logger.LogWarning($"Consumer with ID {userId} not found.");
                    return NotFound();
                }

                return NoContent();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error updating Billing Model for Consumer with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error updating Billing Model for Consumer with ID {userId}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}