using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Data.Common.Contracts;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Helpers;
using Data.Common.Services.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Framework.Utilities;

namespace Data.Common.Services.Tests.RepositoryIntegrationTests.Base
{
    public abstract class RepositoryIntegrationTestsBase<T, TEntityCreator>
        where T : BaseEntity, new()
        where TEntityCreator : CommonEntityCreator
    {
        private IUnitOfWork unitOfWork;
        private TEntityCreator entityCreator;

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

        [TestInitialize]
        public void SetupEachTest()
        {
            DbContext = CreateDbContext();
            unitOfWork = new UnitOfWork(DbContext, new RepositoryFactory(new RepositoryFactoriesBuilder()));
            entityCreator = CreateEntityCreator(DbContext);
        }

        protected abstract DbContext CreateDbContext();

        protected abstract TEntityCreator CreateEntityCreator(DbContext context);

        [TestCleanup]
        public void TearDownEachTest()
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

        [TestMethod]
        public void InsertOrUpdate_NewObjectProvided_CreatedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            Assert.IsTrue(CreatedEntity.CreatedAt != null);
        }

        [TestMethod]
        public void GetById_CreatedEntityIdProvided_RetrievedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            T result = unitOfWork.GetById<T>(CreatedEntity.Id);
            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public async void GetAll_CreatedEntityIdProvided_RetrievedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            IList<T> allEntities = await unitOfWork.GetAll<T>().ToListAsync();
            Assert.IsNotNull(allEntities.FirstOrDefault(x => x.Id == CreatedEntity.Id));
        }

        [TestMethod]
        public void Delete_CreatedEntityProvided_DeletedSuccessfully()
        {
            T entity = CreateEntity();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            unitOfWork.Delete(CreatedEntity);
            SaveChanges();
            BaseEntity result = unitOfWork.GetById<T>(CreatedEntity.Id);
            Assert.IsNull(result);
        }

        protected virtual void InsertOrUpdate(T entity)
        {
            CreatedEntity = unitOfWork.InsertOrUpdate(entity);
            SaveChanges();
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

        protected abstract T CreateEntity();
    }
}