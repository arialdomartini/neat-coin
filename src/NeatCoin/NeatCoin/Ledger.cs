using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly ImmutableList<Page> _pages;

        public Ledger() : this(ImmutableList<Page>.Empty) {}

        private Ledger(ImmutableList<Page> pages)
        {
            _pages = pages;
        }

        public Ledger Append(Page page) =>
            new Ledger(_pages.Add(page));

        public int BalanceOf(string account)
        {
            var balance = 0;
            var currentPage = _pages.SingleOrDefault(p => p.IsRoot);
            while (currentPage != null)
            {
                balance += currentPage.BalanceOf(account);
                currentPage = _pages.SingleOrDefault(p => p.Parent == currentPage.Name);
            }

            return balance;
        }
    }
}