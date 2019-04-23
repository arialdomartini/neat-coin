using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Page
    {
        private readonly ImmutableList<Transaction> _transactions;

        public static Page GetEmpty(string name, string parent) =>
            new Page(name, parent);

        public string Parent { get; }
        public string Name { get; }
        public bool IsRoot => Parent == "";

        private Page(string name, string parent) : this(ImmutableList<Transaction>.Empty, name, parent){}

        private Page(ImmutableList<Transaction> transactions, string name, string parent)
        {
            Name = name;
            Parent = parent;
            _transactions = transactions;
        }

        public Page Append(Transaction transaction) =>
            new Page(_transactions.Add(transaction), Name, Parent);

        public int BalanceOf(string account) =>
            _transactions.Where(t => t.Receiver == account).Sum(t => t.Amount) -
            _transactions.Where(t => t.Sender == account).Sum(t => t.Amount);
    }
}