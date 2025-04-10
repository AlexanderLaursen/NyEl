using API.Repositories.Interfaces;
using Common.Dtos.Consumer;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{

    [ApiController]
    [Route("api/v1/admin/consumers/")]
    public class AdminConsumerController : Controller
    {
        private readonly IConsumerRepository _consumerRepository;
        private readonly IInvoicePreferenceRepository _invoicePreferenceRepository;

        public AdminConsumerController(IConsumerRepository consumerRepository, IInvoicePreferenceRepository invoicePreferenceRepository)
        {
            _consumerRepository = consumerRepository;
            _invoicePreferenceRepository = invoicePreferenceRepository;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsumerByClaims(int id)
        {
            try
            {
                Consumer consumer = await _consumerRepository.GetConsumerByConsumerIdAsync(id);
                List<InvoicePreference> invoicePreferences = await _invoicePreferenceRepository.GetByConsumerIdAsync(id);

                if (consumer == null)
                {
                    return NotFound();
                }

                ConsumerDtoFull consumerDto = new()
                {
                    FirstName = consumer.FirstName ?? "",
                    LastName = consumer.LastName ?? "",
                    PhoneNumber = consumer.PhoneNumber ?? "",
                    Email = consumer.Email ?? "",
                    Address = consumer.Address ?? "",
                    City = consumer.City ?? "",
                    ZipCode = consumer.ZipCode,
                    CPR = consumer.CPR,
                    Id = consumer.Id,
                    BillingModel = consumer.BillingModel.BillingModelType,
                    InvoicePreferences = invoicePreferences.Select(ip => ip.InvoicePreferenceType).ToList()
                };

                return Ok(consumerDto);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (UnkownUserException ex)
            {
                return Unauthorized();
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
