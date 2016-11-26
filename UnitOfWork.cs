using System;
using System.Data.Entity;

namespace ClouDeveloper.EntityFramework
{
    public abstract class UnitOfWork : IDisposable
    {
        protected UnitOfWork(Func<DbContext> contextFactory = null)
        {
            if (contextFactory == null)
                contextFactory = Activator.CreateInstance<DbContext>;

            Context = contextFactory.Invoke();

            if (this.Context == null)
                throw new NullReferenceException("Context cannot be null reference.");
        }

        protected UnitOfWork(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Context = context;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        protected DbContext Context;
        private bool _disposed;

        protected Lazy<GenericRepository<TEntity>> CreateEntityFactory<TEntity>() where TEntity : class
        {
            return new Lazy<GenericRepository<TEntity>>(() => new GenericRepository<TEntity>(Context), false);
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
