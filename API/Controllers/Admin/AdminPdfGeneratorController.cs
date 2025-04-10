using API.HostedServices;
using API.HostedServices.Interfaces;
using Common.Dtos.PdfGenerator;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [ApiController]
    [Route("api/v1/admin/pdf-generator")]
    public class AdminPdfGeneratorController : BaseController
    {
        private readonly PdfGenerationService _pdfGenerationService;

        public AdminPdfGeneratorController(PdfGenerationService pdfGenerationService)
        {
            _pdfGenerationService = pdfGenerationService;
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("full-status")]
        public IActionResult GetFullStatus()
        {
            Guid guid = _pdfGenerationService.GetGuid();

            ServiceStatus status = _pdfGenerationService.GetStatus();
            int queueLength = _pdfGenerationService.GetQueueLength();
            int delay = _pdfGenerationService.GetDelay();
            bool delayActive = _pdfGenerationService.GetDelayActive();
            int queueCheckInterval = _pdfGenerationService.GetQueueCheckInterval();

            var fullStatus = new PdfFullStatus
            {
                GUID = guid,
                Status = status,
                QueueLength = queueLength,
                Delay = delay,
                DelayActive = delayActive,
                QueueCheckInterval = queueCheckInterval
            };

            return Ok(fullStatus);
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(_pdfGenerationService.GetStatus());
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("queue-length")]
        public IActionResult GetQueueLength()
        {
            return Ok(_pdfGenerationService.GetQueueLength());
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("test-delay/time")]
        public IActionResult GetTestDelay()
        {
            return Ok(_pdfGenerationService.GetDelay());
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("test-delay/time")]
        public IActionResult SetTestDelay(DelayDto delayDto)
        {
            if (delayDto == null)
            {
                return BadRequest("Delay value is required.");
            }

            _pdfGenerationService.SetDelay(delayDto.Delay);

            return Ok(true);
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("test-delay/active")]
        public IActionResult GetTestDelayActive()
        {
            return Ok(_pdfGenerationService.GetDelayActive());
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("test-delay/active")]
        public IActionResult SetTestDelayActive(DelayActiveDto delayActiveDto)
        {
            if (delayActiveDto == null)
            {
                return BadRequest("DelayActive value is required.");
            }

            _pdfGenerationService.SetDelayActive(delayActiveDto.DelayActive);

            return Ok(true);
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("queue-interval")]
        public IActionResult GetQueueCheckInterval()
        {
            return Ok(_pdfGenerationService.GetQueueCheckInterval());
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("queue-interval")]
        public IActionResult SetQueueCheckInterval(QueueIntervalDto queueIntervalDto)
        {
            if (queueIntervalDto == null)
            {
                return BadRequest("QueueInterval value is required.");
            }

            _pdfGenerationService.SetQueueCheckInterval(queueIntervalDto.QueueInterval);

            return Ok(true);
        }
    }
}
