using Business.Contracts.Services.Common;
using Data.Contracts;
using Data.Contracts.Entities.Identity;
using Shared.Framework.Security;

namespace Services.Services.Common
{
    public abstract class BaseService: IBaseService
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IIdentityStorage identityStorage;

        protected BaseService(IUnitOfWork unitOfWork, IIdentityStorage identityStorage)
        {
            this.unitOfWork = unitOfWork;
            this.identityStorage = identityStorage;
        }

        public User GetCurrentUser()
        {
            var currentPrincipal = identityStorage.GetPrincipal();
            User currentUser = unitOfWork.GetById<User>(currentPrincipal.UserId);

            return currentUser;
        }
    }
}