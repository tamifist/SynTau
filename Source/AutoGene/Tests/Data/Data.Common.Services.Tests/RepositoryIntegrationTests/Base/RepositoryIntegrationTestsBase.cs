using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Data.Common.Contracts;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Helpers;
using Data.Common.Services.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Shared.Framework.Utilities;
using Xunit;

namespace Data.Common.Services.Tests.RepositoryIntegrationTests.Base
{
    public abstract class RepositoryIntegrationTestsBase<T, TEntityCreator>: IDisposable
        where T : BaseEntity, new()
        where TEntityCreator : CommonEntityCreator
    {
        private IUnitOfWork unitOfWork;
        private TEntityCreator entityCreator;

        protected RepositoryIntegrationTestsBase()
        {
            TestInitialize();
        }

        protected DbContext DbContext
        {
            get;
            private set;
        }

        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return unitOfWork;
            }
        }

        protected TEntityCreator EntityCreator
        {
            get
            {
                return entityCreator;
            }
        }

        protected T CreatedEntity
        {
            get;
            set;
        }

        protected virtual bool EnableDelete
        {
            get
            {
                return true;
            }
        }

        [Fact]
        public void InsertOrUpdate_NewObjectProvided_CreatedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            Assert.True(CreatedEntity.CreatedAt != null);
        }

        [Fact]
        public void GetById_CreatedEntityIdProvided_RetrievedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            T result = unitOfWork.GetById<T>(CreatedEntity.Id);
            Assert.NotNull(entity);
        }

        [Fact]
        public async void GetAll_CreatedEntityIdProvided_RetrievedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            IList<T> allEntities = await unitOfWork.GetAll<T>().ToListAsync();
            Assert.NotNull(allEntities.FirstOrDefault(x => x.Id == CreatedEntity.Id));
        }

        [Fact]
        public void Delete_CreatedEntityProvided_DeletedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            unitOfWork.Delete(CreatedEntity);
            SaveChanges();
            BaseEntity result = unitOfWork.GetById<T>(CreatedEntity.Id);
            Assert.Null(result);
        }

        public void Dispose()
        {
            TestCleanup();
        }

        protected abstract DbContext CreateDbContext();

        protected abstract TEntityCreator CreateEntityCreator(DbContext context);

        protected abstract T CreateEntity();

        protected virtual void InsertOrUpdate(T entity)
        {
            CreatedEntity = unitOfWork.InsertOrUpdate(entity);
            SaveChanges();
        }

        private void TestInitialize()
        {
            DbContext = CreateDbContext();
            unitOfWork = new UnitOfWork(DbContext, new RepositoryFactory(new RepositoryFactoriesBuilder()));
            entityCreator = CreateEntityCreator(DbContext);
        }

        private void TestCleanup()
        {
            if (CreatedEntity != null)
            {
                if (EnableDelete)
                {
                    entityCreator.DeleteCreatedEntities();
                }
                DbContext.SaveChanges();
                CreatedEntity = null;
            }
            DbContext.Dispose();
            unitOfWork = null;
        }

        private void SaveChanges()
        {
            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}