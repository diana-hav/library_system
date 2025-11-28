using Ardalis.Specification;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.Dal.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T?> GetBySpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
