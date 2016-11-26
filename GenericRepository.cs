using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ClouDeveloper.EntityFramework
{
    /// <summary>
    /// Generic repository class for Entity Framework model class.
    /// </summary>
    /// <remarks>
    /// Original source code comes from https://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// </remarks>
    /// <typeparam name="TEntity">Entity framework model class</typeparam>
    public class GenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Context = context;
            DbSet = context.Set<TEntity>();
        }
        
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;
        
        public virtual List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IEnumerable<string> includeProperties = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties ?? new string[] { })
            {
                query = query.Include(includeProperty);
            }

            return orderBy?.Invoke(query).ToList() ?? query.ToList();
        }

        /// <summary>
        /// Get an item with equal identity.
        /// </summary>
        /// <param name="id">ID value.</param>
        /// <returns>Matched entity.</returns>
        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual void Delete(object id)
        {
            Delete(DbSet.Find(id));
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entitiesToDelete)
        {
            var eachEntities = entitiesToDelete as TEntity[] ?? entitiesToDelete.ToArray();

            if (!eachEntities.Any())
                return;

            foreach (TEntity eachEntity in eachEntities)
            {
                if (Context.Entry(eachEntity).State == EntityState.Detached)
                {
                    DbSet.Attach(eachEntity);
                }
            }
            DbSet.RemoveRange(eachEntities);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
