using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly Pages _pages;
        private readonly int _difficulty;
        private readonly int _reward;
        private string LastTransactionHash => _pages.Any() ? _pages.Last().Hash : null;
        public bool IsValid => _pages.IsValid(_difficulty);

        public Ledger(int difficulty, int reward) : this(difficulty, reward, new Pages(ImmutableList<Page>.Empty)) {}

        public Ledger(int difficulty, int reward, Pages pages)
        {
            _reward = reward;
            _difficulty = difficulty;
            _pages = pages;
        }

        public Ledger Append(Page page, string author) => new Ledger(2, 50, Link(page, author));

        private Pages Link(Page page, string author)
        {
            return _pages.Add(
                page
                    .LinkTo(LastTransactionHash)
                    .Reward(author, _reward)
                    .MakeTheHashValid(_difficulty));
        }

        public int Balance(string account) => _pages
            .Sum(p => p.BalanceOf(account));
    }
}