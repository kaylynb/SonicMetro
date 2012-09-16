using System.Threading.Tasks;
using SonicCache.Interfaces;

namespace SonicCache.Data.Interfaces
{
    interface ISubsonicData
    {
        Task GetAsync(SourcePolicy sourcePolicy);

        ISonicCache Cache { get; }
    }
}
