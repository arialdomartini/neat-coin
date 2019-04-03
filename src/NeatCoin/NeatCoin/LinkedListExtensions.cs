using System.Collections.Generic;
using System.Linq;

namespace NeatCoin
{
    public static class LinkedListExtensions
    {
        public static IEnumerable<Page> IterateFrom(this IEnumerable<Page> pages, Page root)
        {
            yield return root;

            var pageList = pages.ToList();
            foreach (var child in pageList.Where(p => p.Parent == root.Hash))
            {
                foreach(var z in IterateFrom(pageList, child))
                {
                    yield return z;
                }
            }
        }
    }
}