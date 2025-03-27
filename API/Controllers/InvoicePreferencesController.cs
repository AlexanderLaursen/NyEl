using System.Security.Claims;
using API.Repositories.Interfaces;
using Common.Dtos.InvoicePreference;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/invoice-preferences")]
    public class InvoicePreferencesController : Controller
    {
        private readonly IInvoicePreferenceRepository _invoicePreferenceRepository;
        private readonly ILogger<InvoicePreferencesController> _logger;

        public InvoicePreferencesController(IInvoicePreferenceRepository invoicePreferenceRepository, ILogger<InvoicePreferencesController> logger)
        {
            _invoicePreferenceRepository = invoicePreferenceRepository;
            _logger = logger;
        }

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
                List<InvoicePreference> invoicePreference = await _invoicePreferenceRepository.GetByUserIdAsync(userId);
                if (invoicePreference == null)
                {
                    _logger.LogWarning($"InvoicePreference with ID {userId} not found.");
                    return NotFound();
                }

                List<InvoicePreferenceEnum> enumList = invoicePreference
                    .Select(ip => ip.InvoiceNotificationPreference).ToList();

                InvoicePreferenceListDto invoicePreferenceDtoList = new()
                {
                    InvoicePreferences = enumList
                };

                return Ok(invoicePreferenceDtoList);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving InvoicePreference with ID {userId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving InvoicePreference with ID {userId}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // TODO Implement preference update
    }
}
