using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace ClouDeveloper.EntityFramework
{
    public abstract class UnitOfWorkAsync : UnitOfWork
    {

        protected UnitOfWorkAsync(Func<DbContext> contextFactory = null) : base(contextFactory) { }

        protected UnitOfWorkAsync(DbContext context = null) : base(context) { }

        protected Lazy<GenericRepositoryAsync<TEntity>> CreateEntityFactoryAsync<TEntity>() where TEntity : class
        {
            return new Lazy<GenericRepositoryAsync<TEntity>>(() => new GenericRepositoryAsync<TEntity>(Context), false);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Context.SaveChangesAsync(cancellationToken);
        }
    }
}
