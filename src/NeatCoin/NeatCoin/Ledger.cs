using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private readonly ImmutableList<Transaction> _transactions = ImmutableList<Transaction>.Empty;

        public Ledger()
        {
            
        }

        private Ledger(ImmutableList<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public Ledger Append(Transaction transaction) => new Ledger(_transactions.Add(transaction));

        public int Balance(string account) =>
            _transactions.Where(t => account == t.Receiver).Sum(t => t.Amount) - 
            _transactions.Where(t => account == t.Sender).Sum(t => t.Amount);
    }
}