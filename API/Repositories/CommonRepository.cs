using API.Data;
using API.Repositories.Interfaces;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        private readonly DataContext _context;
        private readonly ILogger<CommonRepository<T>> _logger;

        public CommonRepository(DataContext context, ILogger<CommonRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving data from the database.");
                throw new RepositoryException("Error occurred while retrieving data from the database.", ex);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving data from the database.");
                throw new RepositoryException("Error occurred while retrieving data from the database.", ex);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing write operation to the database");
                throw new RepositoryException("Error occurred while performing write operation to the database", ex);
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await _context.Set<T>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing write operation to the database");
                throw new RepositoryException("Error occurred while performing write operation to the database", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating an entity in the database");
                throw new RepositoryException("Error occurred while updating an entity in the database", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entityToDelete = await _context.Set<T>().FindAsync(id);
                if (entityToDelete != null)
                {
                    _context.Set<T>().Remove(entityToDelete);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Error occurred while performing a delete operation in the database - entity not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing a delete operation in the database");
                throw new RepositoryException("Error occurred while performing a delete operation in the database", ex);
            }
        }
    }
}

