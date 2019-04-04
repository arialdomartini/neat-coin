using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly ImmutableList<Page> _pages = ImmutableList<Page>.Empty;

        public Ledger()
        {
            
        }

        private Ledger(ImmutableList<Page> pages)
        {
            _pages = pages;
        }

        public Ledger Append(Page page) => new Ledger(_pages.Add(page));

        public int Balance(string account) => _pages.Sum(p => p.BalanceOf(account));
    }
}