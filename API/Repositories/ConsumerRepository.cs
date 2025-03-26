using API.Data;
using API.Repositories.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ConsumerRepository : IConsumerRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ConsumerRepository> _logger;

        public ConsumerRepository(DataContext context, ILogger<ConsumerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
