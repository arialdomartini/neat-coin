using System;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private ImmutableList<Page> Pages { get; }

        public Ledger(ImmutableList<Page> pages)
        {
            Pages = pages;
        }

        public int Balance(string account) =>
            Balance(account, AsReceiver())
            - Balance(account, AsSender());

        private static Func<Transaction, string> AsSender() => t => t.Sender;
        private static Func<Transaction, string> AsReceiver() => t => t.Receiver;

        private int Balance(string account, Func<Transaction, string> p) =>
            Pages.Sum(page =>
                page.Transactions
                    .Where(t => p(t) == account)
                    .Sum(t => t.Amount));
    }
}