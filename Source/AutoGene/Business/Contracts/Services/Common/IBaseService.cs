using Data.Contracts.Entities.Identity;

namespace Business.Contracts.Services.Common
{
    public interface IBaseService
    {
        User GetCurrentUser();
    }
}