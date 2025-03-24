using API.Repositories.Interfaces;
using Common.Dtos.InvoicePreference;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InvoicesController : ControllerBase
    {
        [ApiController]
        [Route("/invoices")]
        public class InvoicePreferencesController : ControllerBase
        {
            private readonly ICommonRepository<InvoicePreference> _invoicePreferenceRepository;
            private readonly ILogger<InvoicePreferencesController> _logger;

            public InvoicePreferencesController(ICommonRepository<InvoicePreference> invoicePreferenceRepository, ILogger<InvoicePreferencesController> logger)
            {
                _invoicePreferenceRepository = invoicePreferenceRepository;
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
                    var invoicePreference = await _invoicePreferenceRepository.GetByIdAsync(id);

                    if (invoicePreference == null)
                    {
                        _logger.LogWarning($"InvoicePreference with ID {id} not found.");
                        return NotFound();
                    }

                    InvoicePreferenceDto invoicePreferenceDto = invoicePreference.Adapt<InvoicePreferenceDto>();
                    return Ok(invoicePreferenceDto);
                }
                catch (RepositoryException ex)
                {
                    _logger.LogError(ex, $"Error retrieving InvoicePreference with ID {id}.");
                    return StatusCode(500, "An error occurred while processing your request.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unexpected error retrieving InvoicePreference with ID {id}.");
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
        }
    }
}
