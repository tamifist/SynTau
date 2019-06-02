using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Services.Extensions
{
    public static class DbSetExtensions
    {
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var existingEntity = dbSet.Where(predicate).SingleOrDefault();
            if (existingEntity == null)
            {
                return dbSet.Add(entity);
            }

            return existingEntity;
        }
    }
}