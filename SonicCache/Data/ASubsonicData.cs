using System;
using System.Threading.Tasks;
using SonicCache.Data.Interfaces;
using SonicCache.Interfaces;
using SonicUtil.Utility;

namespace SonicCache.Data
{
    public abstract class ASubsonicData<T> : BindableBase, ISubsonicData, IEquatable<T>
    {
        protected ASubsonicData(ISonicCache cache)
        {
            ThrowIf.Null(cache, "cache");

            Cache = cache;
        }

        public ISonicCache Cache { get; private set; }

        public abstract Task GetAsync(SourcePolicy sourcePolicy);

        public abstract void Merge(T other);

        public abstract bool Equals(T other);
        public abstract override int GetHashCode();
    }
}