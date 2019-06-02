using System.Linq;
using Data.Contracts.Entities;

namespace Data.Contracts
{
    /// <summary>
    /// Interface for repositories (data access services) of the system.
    /// </summary>
    /// <remarks>
    /// Interface is separated from concrete data access technology implementation.
    /// Interface works only with one type of entity.
    /// </remarks>
    /// <typeparam name="T">Type of entity that repository works with.</typeparam>
    public interface IRepository<T>
        where T : Entity
    {
        /// <summary>
        /// Saves entity in the repository.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Saved entity.</returns>
        T InsertOrUpdate(T entity);

        /// <summary>
        /// Gets all entities for the repository.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets entity of type T from repository by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity by id.</returns>
        T GetById(string id);

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <param name="entity">Entity to Delete.</param>
        void Delete(T entity);
    }
}