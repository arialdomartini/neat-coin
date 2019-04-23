

using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        public ImmutableList<Transaction> Transactions { get; }

        public Ledger() : this(ImmutableList<Transaction>.Empty) {}

        private Ledger(ImmutableList<Transaction> transactions)
        {
            Transactions = transactions;
        }

        public Ledger Append(Transaction transaction) =>
            new Ledger(Transactions.Add(transaction));

        public int BalanceOf(string account) =>
            Transactions.Where(t => t.Receiver == account).Sum(t => t.Amount) -
            Transactions.Where(t => t.Sender == account).Sum(t => t.Amount);
    }
}