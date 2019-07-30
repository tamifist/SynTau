using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Data.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;

namespace Data.Common.Services.Tests.TestData
{
    public class CommonEntityCreator
    {
        private readonly DbContext dbContext;
        private readonly IList<KeyValuePair<BaseEntity, Action>> entityList = new List<KeyValuePair<BaseEntity, Action>>();

        public CommonEntityCreator(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TEntity GetOrCreate<TEntity>(TEntity entity, Action entityRemover) where TEntity : BaseEntity
        {
            BaseEntity existingEntity = entityList.FirstOrDefault(x => x.Key == entity).Key;
            if (existingEntity != null)
            {
                return (TEntity)existingEntity;
            }

            Console.WriteLine("Creating instance of {0}", typeof(TEntity).Name);
            entityList.Add(new KeyValuePair<BaseEntity, Action>(entity, entityRemover));
            return entity;
        }

        public void DeleteCreatedEntities()
        {
            foreach (var entry in entityList)
            {
                entry.Value();
            }
            dbContext.SaveChanges();
        }

        public void RemoveEntity<TEntity>(TEntity entity) where TEntity: BaseEntity
        {
            DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
            bool isEntityExists = dbSet.Find(entity.Id) != null;
            if (isEntityExists)
            {
                dbSet.Remove(entity);
            }
        }

        public User CreateTestUserWithRole(string roleName = null)
        {
            User user = CreateTestUser();

            user.AddRole(CreateRole(roleName ?? "TestRole1", null));

            return GetOrCreate(user, () => RemoveEntity(user));
        }

        public User CreateTestUser()
        {
            var user = new User
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                Password = "12345678",
                PasswordSalt = "test",
                Organization = "Test Organization",
                LabGroup = "Test Lab",
                Country = CountryEnum.Belarus,
            };

            return GetOrCreate(user, () => RemoveEntity(user));
        }

        public Role CreateRole(string roleName, string roleDescription)
        {
            var role = new Role
            {
                Name = roleName,
                Description = roleDescription
            };

            return GetOrCreate(role, () => RemoveEntity(role));
        }
    }
}