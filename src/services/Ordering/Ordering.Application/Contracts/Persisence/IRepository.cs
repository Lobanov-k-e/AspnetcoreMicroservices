using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persisence
{
    public interface IRepository<T> 
        where T : EntityBase
    {

        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
										Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
										string includeString = null,
										bool disableTracking = true);
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
									   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
									   List<Expression<Func<T, object>>> includes = null,
									   bool disableTracking = true);
		Task<T> GetByIdAsync(int id);
		T Add(T entity);
		void Update(T entity);
		void Delete(T entity);

		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
