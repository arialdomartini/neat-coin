using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public static class PageExtensions
    {
        public static IEnumerable<Page> IterateFrom(this ImmutableList<Page> pages, Page root)
        {
            var currentPage = root;
            while (currentPage.Number != 0)
            {
                yield return currentPage;
                var child = pages.FirstOrDefault(p => p.Number == currentPage.Number + 1);
                currentPage = child;
            }
        }
    }
}