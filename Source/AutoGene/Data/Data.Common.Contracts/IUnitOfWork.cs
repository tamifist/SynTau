using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Common.Contracts.Entities;

namespace Data.Common.Contracts
{
    /// <summary>
    /// Contract for DB access unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all cahnges in the DB.
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// Saves entity in the repository of type T.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Saved entity.</returns>
        T InsertOrUpdate<T>(T entity)
            where T : BaseEntity;

        /// <summary>
        /// Gets all entities of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> GetAll<T>()
            where T : BaseEntity;

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        T GetById<T>(string id)
            where T : BaseEntity;

        /// <summary>
        /// Delete entity from repository of type T.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        void Delete<T>(T entity)
            where T : BaseEntity;

        Task DeleteWhere<T>(Expression<Func<T, bool>> predicate = null)
            where T : BaseEntity;

        Task InsertAll<T>(IList<T> entities)
            where T : BaseEntity;
    }
}