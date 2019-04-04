using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly Pages _pages;
        private int LastPageNumber => _pages.Any() ? _pages.Max(p => p.Number) : 0;

        public Ledger()
        {
            _pages = new Pages(ImmutableList<Page>.Empty);
        }

        private Ledger(Pages pages)
        {
            _pages = pages;
        }

        public Ledger Append(Page page) => new Ledger(Link(page));

        private Pages Link(Page page) =>
            _pages.Add(
                new Page(
                    LastPageNumber + 1,
                    page.Transactions));

        public int Balance(string account) => _pages
            .Sum(p => p.BalanceOf(account));
    }
}