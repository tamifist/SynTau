using System.Linq;
using Data.Common.Contracts.Entities;
using Data.Common.Services.Tests.RepositoryIntegrationTests.Base;
using Xunit;

namespace Data.Common.Services.Tests.RepositoryIntegrationTests
{
    public class UserRepositoryTests : CommonRepositoryIntegrationTests<User>
    {
        private const string TestRoleName = "TestRole1";

        protected override User CreateEntity()
        {
            return EntityCreator.CreateTestUserWithRole();
        }

        [Fact]
        public void InsertOrUpdate_NewUserWithOneRoleProvided_UserWithRoleSuccessfullyCreated()
        {
            CreateUserWithRole();
            User createdUser = UnitOfWork.GetById<User>(CreatedEntity.Id);
            Assert.True(createdUser.UserRoles.Any(x => x.Role.Name == TestRoleName));
        }

        [Fact]
        public void InsertOrUpdate_RemoveRole_RemovedSuccessfully()
        {
            CreateUserWithRole();
            CreatedEntity.RemoveRoleByName(TestRoleName);
            InsertOrUpdate(CreatedEntity);
            User createdUser = UnitOfWork.GetById<User>(CreatedEntity.Id);
            Assert.Equal(0, createdUser.UserRoles.Count);
        }

        private void CreateUserWithRole()
        {
            User user = EntityCreator.CreateTestUserWithRole(TestRoleName);
            InsertOrUpdate(user);
        }
    }
}