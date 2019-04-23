using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Page
    {
        private readonly ImmutableList<Transaction> _transactions;
        public static Page Empty => new Page();

        private Page() : this(ImmutableList<Transaction>.Empty){}

        private Page(ImmutableList<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public Page Append(Transaction transaction) =>
            new Page(_transactions.Add(transaction));

        public int BalanceOf(string account) =>
            _transactions.Where(t => t.Receiver == account).Sum(t => t.Amount) -
            _transactions.Where(t => t.Sender == account).Sum(t => t.Amount);
    }
}