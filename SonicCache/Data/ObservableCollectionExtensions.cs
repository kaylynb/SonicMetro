using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SonicUtil.Utility;

namespace SonicCache.Data
{
    public static class ObservableCollectionExtensions
    {
        public static void StableMergeUpdate<T>(this Collection<T> source, IEnumerable<T> update)
            where T : ASubsonicData<T>
        {
            // ReSharper disable PossibleMultipleEnumeration
            ThrowIf.Null(source, "source");
            ThrowIf.Null(update, "update");

            var updateList = update as IList<T> ?? update.ToList();

            var intersection = source.Intersect(updateList).ToList();
            var reverseIntersection = updateList.Intersect(source).ToList();
            var remove = source.Except(intersection).ToList();

            foreach (var x in remove)
                source.Remove(x);

            foreach (var item in intersection.Zip(reverseIntersection, (x, y) => new {Old = x, New = y}))
                item.Old.Merge(item.New);

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