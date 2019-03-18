using System.Collections.Immutable;

namespace NeatCoin
{
    public struct Group
    {
        public ImmutableList<Transaction> Transactions { get; }

        public Group(ImmutableList<Transaction> transactions)
        {
            Transactions = transactions;
        }
    }
}