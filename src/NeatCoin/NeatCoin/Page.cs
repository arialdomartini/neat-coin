using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public struct Page
    {
        public ImmutableList<Transaction> Transactions { get; }

        public Page(params Transaction[] transactions)
        {
            Transactions = ImmutableList.Create(transactions);
        }

        public int BalanceOf(string account) => Transactions.Sum(t => t.BalanceOf(account));
    }
}