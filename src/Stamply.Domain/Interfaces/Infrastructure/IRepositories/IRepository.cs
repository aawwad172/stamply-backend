using System.Linq.Expressions;

using Stamply.Domain.Entities;

namespace Stamply.Domain.Interfaces.Infrastructure.IRepositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<PaginationResult<T>> GetAllAsync(
        int? pageNumber,
        int? pageSize,
        Expression<Func<T, bool>>? filter);
    Task<T> AddAsync(T entity);
    T? Update(T entity);
    Task DeleteAsync(Guid id);
}
