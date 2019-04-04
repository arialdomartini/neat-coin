using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly Pages _pages;

        public Ledger()
        {
            _pages = new Pages(ImmutableList<Page>.Empty);
        }

        private Ledger(Pages pages)
        {
            _pages = pages;
        }

        public Ledger Append(Page page) => new Ledger(_pages.Add(page));

        public int Balance(string account) => _pages
            .Sum(p => p.BalanceOf(account));
    }
}