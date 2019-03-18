using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static NeatCoinTest.Transaction;

namespace NeatCoinTest
{
    internal class Wallet
    {
        private readonly ImmutableList<Transaction> _transactions;

        private Wallet(Transaction transaction)
        {
            _transactions = new List<Transaction>
            {
                transaction
            }.ToImmutableList();
        }

        public Wallet()
        {
            _transactions = ImmutableList.Create<Transaction>();
        }

        internal Wallet Push(Transaction transaction) =>
            new Wallet(transaction);

        public int BalanceOf(Account account) =>
            Total(IsReceiver(account)) - Total(IsSender(account));

        private int Total(Func<Transaction, bool> condition) =>
            _transactions
                .Where(condition)
                .Sum(t => t.Amount);
    }
}