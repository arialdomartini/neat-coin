

using System;
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

        public int BalanceOf(string account) => _pages.Sum(p => p.BalanceOf(account));

    }
}