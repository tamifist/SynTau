using System.Linq;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Helpers;
using Data.Common.Services.Tests.RepositoryIntegrationTests.Base;
using Data.Common.Services.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Common.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class UserRepositoryTests : CommonRepositoryIntegrationTests<User>
    {
        private const string TestRoleName = "TestRole1";

        protected override User CreateEntity()
        {
            return EntityCreator.CreateTestUser();
        }

        [TestMethod]
        public void InsertOrUpdate_NewUserWithOneRoleProvided_UserWithRoleSuccessfullyCreated()
        {
            CreateUserWithRole();
            User createdUser = UnitOfWork.GetById<User>(CreatedEntity.Id);
            Assert.IsTrue(createdUser.UserRoles.Any(x => x.Role.Name == TestRoleName));
        }

        [TestMethod]
        public void InsertOrUpdate_RemoveRole_RemovedSuccessfully()
        {
            CreateUserWithRole();
            CreatedEntity.RemoveRoleByName(TestRoleName);
            InsertOrUpdate(CreatedEntity);
            User createdUser = UnitOfWork.GetById<User>(CreatedEntity.Id);
            Assert.AreEqual(0, createdUser.UserRoles.Count);
        }

        private void CreateUserWithRole()
        {
            User user = EntityCreator.CreateTestUserWithRole(TestRoleName);
            InsertOrUpdate(user);
        }
    }
}