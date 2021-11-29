using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persisence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persisence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository
{
    public class RepositoryBase<T> : IRepository<T>
        where T : EntityBase
    {
        private readonly OrderContext _context;
        protected readonly DbSet<T> _set;

        public RepositoryBase(OrderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<T>();
        }

        public T Add(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _set.Add(entity);         

            return entity;
        }

        public void Update(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _set.Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
           return await _set.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAsync(predicate, orderBy: null,  includeString: null, disableTracking: false);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            var query = _set as IQueryable<T>;

            if (disableTracking)
                query = query.AsNoTracking();

            if (string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            var query = _set as IQueryable<T>;

            if (disableTracking)
                query = query.AsNoTracking();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
