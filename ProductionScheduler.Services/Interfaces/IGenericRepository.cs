using System.Linq.Expressions;

namespace ProductionScheduler.Services.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task AddAsync(T entity);
    void Update(T entity);
    Task Delete(int id);
    Task<IEnumerable<T>> GetAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> GetOneAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate);
}
