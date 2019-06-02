using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Shared.DTO.Responses;

namespace AutoGene.Mobile.Abstractions
{
    public interface ICloudService
    {
        MobileServiceClient Client { get; set; }

        Task<ICloudTable<T>> GetTableAsync<T>() where T : BaseDTO;

        Task SyncOfflineCacheAsync();
    }
}