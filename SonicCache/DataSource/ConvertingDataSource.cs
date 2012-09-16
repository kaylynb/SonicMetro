using System;
using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Utility;

namespace SonicCache.DataSource
{
    public class ConvertingDataSource<TIn, TOut, TOutKey> : IDataSource<TOut>
    {
        private readonly IDataSource<TIn, TOutKey> _dataSource;

        private readonly Func<TIn, Task<TOut>> _conversionFunc;
        private readonly Func<Task<TOutKey>> _keyGenerationFunc;

        public ConvertingDataSource(Func<TIn, Task<TOut>> conversionFunc, Func<Task<TOutKey>> keyGenerationFunc, IDataSource<TIn, TOutKey> dataSource)
        {
            ThrowIf.Null(conversionFunc, "conversionFunc");
            ThrowIf.Null(keyGenerationFunc, "keyGenerationFunc");
            ThrowIf.Null(dataSource, "dataSource");

            _conversionFunc = conversionFunc;
            _keyGenerationFunc = keyGenerationFunc;
            _dataSource = dataSource;
        }

        public async Task<TOut> GetAsync(SourcePolicy sourcePolicy)
        {
            return await _conversionFunc(await _dataSource.GetAsync(await _keyGenerationFunc().ConfigureAwait(false), sourcePolicy).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }

    public class ConvertingDataSource<TIn, TOut, TInKey, TOutKey> : IDataSource<TOut, TInKey>
    {
        private readonly IDataSource<TIn, TOutKey> _dataSource;

        private readonly Func<TIn, Task<TOut>> _conversionFunc;
        private readonly Func<TInKey, Task<TOutKey>> _keyConversionFunc; 

        public ConvertingDataSource(Func<TIn, Task<TOut>> conversionFunc, Func<TInKey, Task<TOutKey>> keyConversionFunc, IDataSource<TIn, TOutKey> dataSource) 
        {
            ThrowIf.Null(conversionFunc, "conversionFunc");
            ThrowIf.Null(keyConversionFunc, "keyConversionFunc");
            ThrowIf.Null(dataSource, "dataSource");

            _conversionFunc = conversionFunc;
            _keyConversionFunc = keyConversionFunc;
            _dataSource = dataSource;
        }

        public async Task<TOut> GetAsync(TInKey key, SourcePolicy sourcePolicy)
        {
            return await _conversionFunc(await _dataSource.GetAsync(await _keyConversionFunc(key).ConfigureAwait(false), sourcePolicy).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
