using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ProductionScheduler.Services.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbSet = dbContext.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Attach(entity);
    }

    public async Task Delete(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
}
