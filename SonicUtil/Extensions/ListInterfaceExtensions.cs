using System.Collections.Generic;
using System.Linq;
using SonicUtil.Utility;

namespace SonicUtil.Extensions
{
    public static class ListInterfaceExtensions
    {
        public static void StableMerge<T>(this IList<T> source, IEnumerable<T> update)
        {
            // ReSharper disable PossibleMultipleEnumeration
            ThrowIf.Null(source, "source");
            ThrowIf.Null(update, "update");

            var updateList = update as IList<T> ?? update.ToList();

            var intersection = source.Intersect(updateList).ToList();
            var remove = source.Except(intersection).ToList();

            foreach (var x in remove)
                source.Remove(x);

            var sourcei = 0;
            var updatei = 0;

            while (sourcei < source.Count && updatei < updateList.Count)
            {
                if (!source[sourcei].Equals(updateList[updatei]))
                    source.Insert(sourcei, updateList[updatei]);

                ++sourcei;
                ++updatei;
            }

            while (updatei < updateList.Count)
            {
                source.Add(updateList[updatei]);
                ++updatei;
            }
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
