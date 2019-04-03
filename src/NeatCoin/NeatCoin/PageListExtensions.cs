using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public static class PageListExtensions
    {
        public static Page GetRoot(this ImmutableList<Page> pages) => pages.Single(p => p.IsRoot);
    }
}