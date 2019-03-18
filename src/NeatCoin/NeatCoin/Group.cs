using System.Collections.Immutable;

namespace NeatCoin
{
    public class Group
    {
        public ImmutableList<Transaction> Transactions { get; }

        public Group(ImmutableList<Transaction> transactions)
        {
            Transactions = transactions;
        }
    }
}