using System.Threading.Tasks;

namespace ApiRequestManager.Interfaces
{
    public interface IMemoryCacheService
    {

        T Get<T>(string key);

        Task<T> GetAsync<T>(string key);

        void Set<T>(string key, object entry);


        void Remove<T>(string key);

    }
}