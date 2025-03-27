using System.Security.Claims;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Dtos.InvoicePreference;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/invoice-preferences")]
    public class InvoicePreferencesController : Controller
    {
        private readonly IInvoicePreferenceRepository _invoicePreferenceRepository;
        private readonly ILogger<InvoicePreferencesController> _logger;
        private readonly IConsumerService _consumerService;

        public InvoicePreferencesController(IInvoicePreferenceRepository invoicePreferenceRepository, ILogger<InvoicePreferencesController> logger,
            IConsumerService consumerService)
        {
            _invoicePreferenceRepository = invoicePreferenceRepository;
            _logger = logger;
            _consumerService = consumerService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetByUserClaims()
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int consumerId = await _consumerService.GetConsumerId(userId);

                List<InvoicePreference> invoicePreference = await _invoicePreferenceRepository.GetByConsumerIdAsync(consumerId);
                if (invoicePreference == null)
                {
                    _logger.LogWarning($"InvoicePreference with ID {userId} not found.");
                    return NotFound();
                }

                List<InvoicePreferenceType> enumList = invoicePreference
                    .Select(ip => ip.InvoicePreferenceType).ToList();

                InvoicePreferenceListDto invoicePreferenceDtoList = new()
                {
                    InvoicePreferences = enumList
                };

                return Ok(invoicePreferenceDtoList);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving InvoicePreference.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving InvoicePreference.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> UpdatePreferences(InvoicePreferenceListDto preferencesDto)
        {
            if (preferencesDto.InvoicePreferences.Count() == 0)
            {
                return BadRequest();
            }

            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int consumerId = await _consumerService.GetConsumerId(userId);

                int succesfulChanges = await _invoicePreferenceRepository.UpdateInvoicePreferences(preferencesDto.InvoicePreferences, consumerId);
            
                if (succesfulChanges == 0)
                {
                    _logger.LogWarning($"InvoicePreference with ID {consumerId} not found.");
                    return NotFound();
                }

                return Ok();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error updating InvoicePreference.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error updating InvoicePreference.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
