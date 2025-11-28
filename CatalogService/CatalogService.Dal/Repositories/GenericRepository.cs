using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogService.Dal.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<T?> GetBySpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec).CountAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        protected IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec);
        }
    }
}
