using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Data.Contracts;
using Data.Contracts.Entities;
using Data.Framework.Helpers;
using Data.Services.Helpers;
using Data.Services.Tests.TestData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Framework.Utilities;

namespace Data.Services.Tests.RepositoryIntegrationTests.Base
{
    [TestClass]
    public abstract class RepositoryIntegrationTestsBase<T, TDataCreator>
        where T : Entity, new()
        where TDataCreator : TestDataCreator
    {
        private IUnitOfWork unitOfWork;
        private TDataCreator creator;

        protected EntityContext DbContext
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

        protected TDataCreator EntityCreator
        {
            get
            {
                return creator;
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
            DbContext = new CommonDbContext();
            unitOfWork = new UnitOfWork(DbContext, new RepositoryFactory(new RepositoryFactoriesBuilder()));
            creator = CreateEntityCreator(DbContext);
        }

        [TestCleanup]
        public void TearDownEachTest()
        {
            if (CreatedEntity != null)
            {
                if (EnableDelete)
                {
                    creator.DeleteCreatedEntities();
                }
                DbContext.SaveChanges();
                CreatedEntity = null;
            }
            DbContext.Dispose();
            unitOfWork = null;
        }

        // This is tests for CRUD. All we need in our tests class is override 
        // CreateEntity method to provide specific instance of Entity with own mapping.
        [TestMethod]
        public void InsertOrUpdate_NewObjectProvided_CreatedSuccessfully()
        {
            T entity = CreateEntity();
            creator.InitializeGraph();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            AssertOnCreatedEntity(CreatedEntity);
        }

        [TestMethod]
        public void GetById_CreatedEntityIdProvided_RetrievedSuccessfully()
        {
            T entity = CreateEntity();
            creator.InitializeGraph();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            T result = unitOfWork.GetById<T>(CreatedEntity.Id);
            AssertOnRetrievedEntity(result);
        }

        [TestMethod]
        public void Delete_CreatedEntityProvided_DeletedSuccessfully()
        {
            T entity = CreateEntity();
            creator.InitializeGraph();
            InsertOrUpdate(entity);
            Console.WriteLine(CreatedEntity.DumpToString());
            unitOfWork.Delete(CreatedEntity);
            SaveChanges();
            Entity result = unitOfWork.GetById<T>(CreatedEntity.Id);
            Assert.IsNull(result);
        }

        protected virtual void AssertOnCreatedEntity(T entity)
        {
            Assert.IsTrue(!(entity.CreatedAt == null));
        }

        protected virtual void AssertOnRetrievedEntity(T entity)
        {
            Assert.IsNotNull(entity);
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
            catch (DbEntityValidationException e)
            {
                e.EntityValidationErrors.SelectMany(error => error.ValidationErrors).ToList().ForEach(
                    item => Console.WriteLine("{0} - {1}", item.PropertyName, item.ErrorMessage));
                throw;
            }
        }

        protected abstract TDataCreator CreateEntityCreator(EntityContext context);

        protected abstract T CreateEntity();
    }
}