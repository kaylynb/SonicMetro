using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SonicUtil.Utility;

namespace SonicUtil.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void Update<T>(this ObservableCollection<T> source, IEnumerable<T> update)
        {
            // ReSharper disable PossibleMultipleEnumeration
            ThrowIf.Null(source, "source");
            ThrowIf.Null(update, "update");

            var updateList = update as IList<T> ?? update.ToList();

            var intersection = source.Intersect(updateList).ToList();
            var add = updateList.Except(intersection).ToList();
            var remove = source.Except(intersection).ToList();

            foreach(var x in add)
                source.Add(x);

            foreach (var x in remove)
                source.Remove(x);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}