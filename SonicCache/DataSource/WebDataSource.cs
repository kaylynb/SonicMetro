using System;
using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Utility;

namespace SonicCache.DataSource
{
    public class WebDataSource<TRestData> : IDataSource<TRestData, Uri>
    {
        private readonly SonicAPI.Query _query;

        public WebDataSource(SonicAPI.Query query)
        {
            ThrowIf.Null(query, "query");

            _query = query;
        }

        public async Task<TRestData> GetAsync(Uri requestUri, SourcePolicy sourcePolicy)
        {
            ThrowIf.Null(requestUri, "requestUri");

            return await _query.GetAsync<TRestData>(requestUri).ConfigureAwait(false);
        }
    }
}