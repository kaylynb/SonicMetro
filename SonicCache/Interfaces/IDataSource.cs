using System.Threading.Tasks;

namespace SonicCache.Interfaces
{
    public enum SourcePolicy
    {
        Cache,
        Refresh
    }

    public interface IDataSource<T, TKey>
    {
        Task<T> GetAsync(TKey key, SourcePolicy sourcePolicy);
    }

    public interface IDataSource<T>
    {
        Task<T> GetAsync(SourcePolicy sourcePolicy);
    }
}