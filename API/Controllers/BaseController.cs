using System.Security.Claims;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public abstract class BaseController : Controller
    {
       private readonly IConsumerService _consumerService;

        public BaseController(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        public BaseController()
        {
        }

        protected virtual async Task<int> GetConsumerId()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }

            int consumerId = await _consumerService.GetConsumerId(userId);
            if (consumerId == 0)
            {
                throw new UnauthorizedAccessException();
            }

            return consumerId;
        }
    }
}
