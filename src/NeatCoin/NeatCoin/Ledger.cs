using System;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        private Page Page { get; }

        public Ledger(Page page)
        {
            Page = page;
        }

        public int Balance(string account) =>
            Balance(account, AsReceiver())
            - Balance(account, AsSender());

        private static Func<Transaction, string> AsSender() => t => t.Sender;
        private static Func<Transaction, string> AsReceiver() => t => t.Receiver;

        private int Balance(string account, Func<Transaction, string> p) =>
            Page.Sum(page =>
                page.Transactions
                    .Where(t => p(t) == account)
                    .Sum(t => t.Amount));
    }
}