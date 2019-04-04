using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public struct Page
    {
        public int Number { get; }
        private ImmutableList<Transaction> Transactions { get; }

        public Page(int number, params Transaction[] transactions)
        {
            Number = number;
            Transactions = ImmutableList.Create(transactions);
        }

        public int BalanceOf(string account) => Transactions.Sum(t => t.BalanceOf(account));
    }
}