using System.Linq;
using Data.Contracts.Entities.Identity;
using Data.Framework.Helpers;
using Data.Services.Helpers;
using Data.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class UserRepositoryTests : RepositoryIntegrationTests<User>
    {
        private const string TestUserEmail = "TestUser999@gmail.com";
        private const string TestUserFirstName = "TestFirstName999";
        private const string TestUserLastName = "TestLastName999";
        private const string TestUserPassword = "1q2w3e!@";
        private const string TestRoleName = "TestRole1";

        protected override User CreateEntity()
        {
            return EntityCreator.CreateUser(TestUserEmail, TestUserFirstName, TestUserLastName, TestUserPassword, TestRoleName, null);
        }

        [TestMethod]
        public void GetAll_RetrievedSuccessfully()
        {
            CreateUserWithRole();
            var dbContext = new CommonDbContext();
            var unitOfWork = new UnitOfWork(dbContext, new RepositoryFactory(new RepositoryFactoriesBuilder()));
            var results = unitOfWork.GetAll<User>().ToList();
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void InsertOrUpdate_NewUserWithOneRoleProvided_UserWithRoleSuccessfullyCreated()
        {
            CreatedEntity = CreateUserWithRole();
            Assert.AreEqual(1, CreatedEntity.Roles.Count);
        }

        [TestMethod]
        public void InsertOrUpdate_RemoveRole_RemovedSuccessfully()
        {
            var user = CreateUserWithRole();
            user.RemoveRoleByName(TestRoleName);
            InsertOrUpdate(user);
            CreatedEntity = user;
            Assert.AreEqual(0, CreatedEntity.Roles.Count);
        }

        private User CreateUserWithRole()
        {
            User user = CreateEntity();
            InsertOrUpdate(user);
            return user;
        }
    }
}