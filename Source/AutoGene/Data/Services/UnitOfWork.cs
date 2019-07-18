using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Contracts;
using Data.Contracts.Entities;
using Data.Framework.Helpers;
using Data.Services.Helpers;
using EntityFramework.BulkInsert.Extensions;
using Microsoft.Azure.Mobile.Server;
using Shared.Framework.Dependency;
using Z.EntityFramework.Plus;

namespace Data.Services
{
    /// <summary>
    /// Represents wrapper above DBContext instance and repositories container.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IScopedDependency
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
            where T : Entity
        {
            return GetRepository<T>().InsertOrUpdate(entity);
        }

        /// <summary>
        /// Gets all entities of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> GetAll<T>()
            where T : Entity
        {
            return GetRepository<T>().GetAll();
        }

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        public T GetById<T>(string id)
            where T : Entity
        {
            return GetRepository<T>().GetById(id);
        }

        /// <summary>
        /// Delete entity from repository of type T.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        public void Delete<T>(T entity)
            where T : Entity
        {
            GetRepository<T>().Delete(entity);
        }

        public Task DeleteWhere<T>(Expression<Func<T, bool>> predicate = null) 
            where T : Entity
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

        public Task InsertAll<T>(IEnumerable<T> entities) 
            where T : Entity
        {
            return dbContext.BulkInsertAsync(entities);
            //return Task.FromResult(0);
        }

        private IRepository<T> GetRepository<T>()
            where T : Entity
        {
            return repositoryFactory.GetRepositoryForEntityType<T>();
        }
    }
}