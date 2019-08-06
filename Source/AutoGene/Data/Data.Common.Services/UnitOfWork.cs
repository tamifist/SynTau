using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Common.Contracts;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Helpers;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Framework.Dependency;
using Z.EntityFramework.Plus;

namespace Data.Common.Services
{
    /// <summary>
    /// Represents wrapper above DBContext instance and repositories container.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Determines if automatic commit on dispose should be performed. If false, Commit must be called manually.
        /// </summary>
        public virtual bool AutoCommitEnabled
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes new instance using given repositoryProvider.
        /// </summary>
        public UnitOfWork(DbContext dbContext, IRepositoryFactory repositoryFactory)
        {
            this.dbContext = dbContext;

            repositoryFactory.DbContext = dbContext;
            this.repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Commits all changes on the DB.
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        public int Commit()
        {
            int countOfAffectedRows = -1;

            try
            {
                countOfAffectedRows = dbContext.SaveChanges();
                //dbContext.BulkSaveChanges();
            }
            catch (Exception exception)
            {
                throw;
            }

            return countOfAffectedRows;
        }

        /// <summary>
        /// Ensures that context is closed and transaction is completed.
        /// </summary>
        public void Dispose()
        {
            if (dbContext != null)
            {
                if (AutoCommitEnabled)
                {
                    Commit();
                }
                dbContext.Dispose();
            }
        }

        /// <summary>
        /// Saves entity in the repository of type T.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Saved entity.</returns>
        public T InsertOrUpdate<T>(T entity)
            where T : BaseEntity
        {
            return GetRepository<T>().InsertOrUpdate(entity);
        }

        /// <summary>
        /// Gets all entities of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> GetAll<T>()
            where T : BaseEntity
        {
            return GetRepository<T>().GetAll();
        }

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        public T GetById<T>(string id)
            where T : BaseEntity
        {
            return GetRepository<T>().GetById(id);
        }

        /// <summary>
        /// Delete entity from repository of type T.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        public void Delete<T>(T entity)
            where T : BaseEntity
        {
            GetRepository<T>().Delete(entity);
        }

        public Task DeleteWhere<T>(Expression<Func<T, bool>> predicate = null) 
            where T : BaseEntity
        {
            var dbSet = dbContext.Set<T>();
            if (predicate != null)
            {
                //return dbContext.BulkDeleteAsync(dbSet.Where(predicate));
                return dbSet.Where(predicate).DeleteAsync();
            }
            else
            {
                //return dbContext.BulkDeleteAsync(dbSet);
                return dbSet.DeleteAsync();
            }
        }

        public Task InsertAll<T>(IList<T> entities) 
            where T : BaseEntity
        {
            return dbContext.BulkInsertAsync(entities);
        }

        private IRepository<T> GetRepository<T>()
            where T : BaseEntity
        {
            return repositoryFactory.GetRepositoryForEntityType<T>();
        }
    }
}