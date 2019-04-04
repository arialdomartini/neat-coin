using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    internal class Pages : IEnumerable<Page>
    {
        private readonly ImmutableList<Page> _pages;

        internal Pages(ImmutableList<Page> pages)
        {
            _pages = pages;
        }

        public Pages Add(Page page) => new Pages(_pages.Add(page));

        public IEnumerator<Page> GetEnumerator()
        {
            var currentPage = _pages.SingleOrDefault(p => p.Number == 1);
            while (currentPage.Number != 0)
            {
                yield return currentPage;
                var child = _pages.FirstOrDefault(p => p.Number == currentPage.Number + 1);
                currentPage = child;
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}