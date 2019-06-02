using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DTO.Responses;

namespace AutoGene.Mobile.Abstractions
{
    public interface ICloudTable<T> where T : BaseDTO
    {
        Task<T> CreateItemAsync(T item);
        Task<T> ReadItemAsync(string id);
        Task<T> UpdateItemAsync(T item);
        Task<T> UpsertItemAsync(T item);
        Task DeleteItemAsync(T item);
        Task<ICollection<T>> ReadAllItemsAsync();
        Task<ICollection<T>> ReadItemsAsync(int start, int count);
        Task PullAsync();
    }
}