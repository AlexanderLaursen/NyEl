﻿using API.Data;
using API.Repositories.Interfaces;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ConsumptionRepository : IConsumptionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ConsumptionRepository> _logger;

        public ConsumptionRepository(DataContext context, ILogger<ConsumptionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ConsumptionReading>> GetConsumptionAsync(int consumerId, Timeframe timeframe)
        {
            try
            {
                return await _context.ConsumptionReadings
                    .Where(cr => cr.Timestamp >= timeframe.Start
                    && cr.Timestamp <= timeframe.End
                    && cr.ConsumerId == consumerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving consumption readings from the database.");
                throw new RepositoryException("Error occurred while retrieving consumption readings from the database.", ex);
            }
        }

        public async Task<int> AddAsync(ConsumptionReading consumptionReading)
        {
            try
            {
                _context.ConsumptionReadings.Add(consumptionReading);
                int changes = await _context.SaveChangesAsync();

                return changes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding consumption readings to the database.");
                throw new RepositoryException("Error occurred while adding consumption readings to the database.", ex);
            }
        }

        public async Task<int> AddRangeAsync(List<ConsumptionReading> consumptionReading)
        {
            try
            {
                _context.ConsumptionReadings.AddRange(consumptionReading);
                int changes = await _context.SaveChangesAsync();

                return changes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding consumption readings to the database.");
                throw new RepositoryException("Error occurred while adding consumption readings to the database.", ex);
            }
        }
    }
}
