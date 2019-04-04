using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly Pages _pages;
        private readonly int _difficulty;
        private string LastTransactionHash => _pages.Any() ? _pages.Last().Hash : null;
        public bool IsValid => _pages.IsValid(_difficulty);

        public Ledger(int difficulty) : this(difficulty, new Pages(ImmutableList<Page>.Empty)) {}

        public Ledger(int difficulty, Pages pages)
        {
            _difficulty = difficulty;
            _pages = pages;
        }

        public Ledger Append(Page page) => new Ledger(2, Link(page));

        private Pages Link(Page page) =>
            _pages.Add(
                page
                    .LinkTo(LastTransactionHash)
                    .Validate(_difficulty));

        public int Balance(string account) => _pages
            .Sum(p => p.BalanceOf(account));
    }
}