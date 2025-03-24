using API.Repositories.Interfaces;
using Common.Dtos.Consumer;
using Common.Exceptions;
using Common.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumersController : ControllerBase
    {
        private readonly ICommonRepository<Consumer> _consumerRepository;
        private readonly ILogger<ConsumersController> _logger;

        public ConsumersController(ICommonRepository<Consumer> consumerRepository, ILogger<ConsumersController> logger)
        {
            _consumerRepository = consumerRepository;
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
                var consumer = await _consumerRepository.GetByIdAsync(id);

                if (consumer == null)
                {
                    _logger.LogWarning($"Consumer with ID {id} not found.");
                    return NotFound();
                }

                ConsumerDto consumerDto = consumer.Adapt<ConsumerDto>();
                return Ok(consumerDto);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Error retrieving Consumer with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving Consumer with ID {id}.");
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
                await _consumerRepository.AddAsync(consumer);
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
    }
}