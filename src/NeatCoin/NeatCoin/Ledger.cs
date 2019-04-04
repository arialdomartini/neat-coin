using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly Pages _pages;
        private string LastTransactionHash => _pages.Any() ? _pages.Last().Hash : null;

        public Ledger()
        {
            _pages = new Pages(ImmutableList<Page>.Empty);
        }

        private Ledger(Pages pages)
        {
            _pages = pages;
        }

        public Ledger Append(Page page) => new Ledger(Link(page));

        private Pages Link(Page page)
        {
            var lastTransactionHash = LastTransactionHash;
            return _pages.Add(
                new Page(
                    lastTransactionHash,
                    page.Transactions));
        }

        public int Balance(string account) => _pages
            .Sum(p => p.BalanceOf(account));
    }
}