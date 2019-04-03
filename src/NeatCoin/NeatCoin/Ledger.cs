using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        public ImmutableList<Page> Pages;

        private Ledger(ImmutableList<Page> pages)
        {
            Pages = pages;
        }

        public Ledger(params Page[] pages) : this(ImmutableList.Create(pages)) { }

        public int Balance(string account) =>
            Pages
                .IterateFrom(Pages.GetRoot())
                .Sum(b => b.Balance(account));
    }
}