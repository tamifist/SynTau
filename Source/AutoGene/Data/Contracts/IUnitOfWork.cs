using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Contracts.Entities;

namespace Data.Contracts
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
            where T : Entity;

        /// <summary>
        /// Gets all entities of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> GetAll<T>()
            where T : Entity;

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        T GetById<T>(string id)
            where T : Entity;

        /// <summary>
        /// Delete entity from repository of type T.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        void Delete<T>(T entity)
            where T : Entity;

        Task DeleteWhere<T>(Expression<Func<T, bool>> predicate = null)
            where T : Entity;

        Task InsertAll<T>(IEnumerable<T> entities)
            where T : Entity;
    }
}