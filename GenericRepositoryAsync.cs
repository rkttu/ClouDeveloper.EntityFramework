using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ClouDeveloper.EntityFramework
{
    // https://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    public class GenericRepositoryAsync<TEntity> : GenericRepository<TEntity>
        where TEntity : class
    {
        public GenericRepositoryAsync(DbContext context) : base(context)
        {
        }

        public virtual Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IEnumerable<string> includeProperties = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties ?? new string[] { }) {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query).ToListAsync(cancellationToken) : query.ToListAsync(cancellationToken);
        }

        public virtual Task<TEntity> GetByIdAsync(object id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbSet.FindAsync(cancellationToken, id);
        }

        public virtual Task DeleteAsync(object id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbSet.FindAsync(cancellationToken, id).ContinueWith(Delete, cancellationToken);
        }
    }
}
