using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Utility;

namespace SonicCache.DataSource
{
    public class CoverArtDataSource : IDataSource<byte[], string>
    {
        private readonly SonicAPI.Query _query;

        public CoverArtDataSource(SonicAPI.Query query)
        {
            ThrowIf.Null(query, "query");

            _query = query;
        }

        public async Task<byte[]> GetAsync(string key, SourcePolicy sourcePolicy)
        {
            var response = await _query.GetCoverArtAsync(key);

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
