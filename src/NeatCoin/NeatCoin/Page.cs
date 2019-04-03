using System.Collections.Immutable;

namespace NeatCoin
{
    public class Page
    {
        public ImmutableList<Transaction> Transactions { get; }

        public Page(ImmutableList<Transaction> transactions)
        {
            Transactions = transactions;
        }
    }
}