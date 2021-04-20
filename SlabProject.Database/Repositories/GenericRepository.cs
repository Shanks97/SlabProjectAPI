using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SlabProject.Database.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        public DbContext Context { get; set; }
        private DbSet<TEntity> DbSet => Context.Set<TEntity>();

        public GenericRepository(DbContext context)
        {
            this.Context = context;
        }

        public TEntity Insert(TEntity entity)
        {
            var result = DbSet.Add(entity).Entity;
            return result;
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        /// <summary>
        /// Use it with lambda expresions
        /// </summary>
        /// <param name="filter">Lambda expression to get the entity</param>
        /// <param name="includeProperties">Include in string csv</param>
        /// <returns></returns>

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this.DbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return query;
        }
    }
}