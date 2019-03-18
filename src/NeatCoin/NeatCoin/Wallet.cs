using System;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Wallet
    {
        private readonly ImmutableList<Transaction> _transactions;

        private Wallet(ImmutableList<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public Wallet()
        {
            _transactions = ImmutableList.Create<Transaction>();
        }

        public Wallet Push(Transaction transaction) =>
            new Wallet(_transactions.Add(transaction));

        public int BalanceOf(Account account) =>
            Total(Transaction.IsReceiver(account)) - Total(Transaction.IsSender(account));

        private int Total(Func<Transaction, bool> condition) =>
            _transactions
                .Where(condition)
                .Sum(t => t.Amount);
    }
}