using System;
using System.Linq;
using Data.Common.Contracts;
using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Common.Services
{
    /// <summary>
    /// Generic Repository implementation for EntityFramework.
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private const string DbContextParameterName = "dbContext";
        private const string DbContextParameterErrorMessage = "dbContext should not be null";

        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(DbContextParameterName, DbContextParameterErrorMessage);
            }
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        protected DbContext DbContext
        {
            get;
            set;
        }

        protected DbSet<T> DbSet
        {
            get;
            set;
        }

        /// <summary>
        /// Saves entity in the repository.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Saved entity.</returns>
        public T InsertOrUpdate(T entity)
        {
            if (IsNew(entity))
            {
                DbSet.Add(entity);
                return entity;
            }

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Gets all entities for the repository.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        public virtual T GetById(string id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        public void Delete(T entity)
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
        }

        protected virtual bool IsNew(T entity)
        {
            return entity.CreatedAt == null;
        }
    }
}