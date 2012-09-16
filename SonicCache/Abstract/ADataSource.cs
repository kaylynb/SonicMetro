using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Utility;

namespace SonicCache.Abstract
{
    public abstract class ADataSource<T> : IDataSource<T>
    {
        protected IDataSource<T> DataSource { get; private set; }

        protected ADataSource(IDataSource<T> dataSource)
        {
            ThrowIf.Null(dataSource, "dataSource");

            DataSource = dataSource;
        }

        public abstract Task<T> GetAsync(SourcePolicy sourcePolicy);
    }

    public abstract class ADataSource<T, TKey> : IDataSource<T, TKey>
    {
        protected IDataSource<T, TKey> DataSource { get; private set; }

        protected ADataSource(IDataSource<T, TKey> dataSource)
        {
            ThrowIf.Null(dataSource, "dataSource");

            DataSource = dataSource;
        }

        public abstract Task<T> GetAsync(TKey key, SourcePolicy sourcePolicy);
    }
}
