using System;

namespace NeatCoinTest
{
    public class Transaction
    {
        public Account Sender { get; }
        public Account Account { get; }
        public Amount Amount { get; }

        public Transaction(Account sender, Account account, Amount amount)
        {
            Sender = sender;
            Account = account;
            Amount = amount;
        }

        public static Func<Transaction, bool> IsSender(Account sender) =>
            t => t.Sender == sender;

        public static Func<Transaction, bool> IsReceiver(Account account) =>
            t => t.Account == account;
    }
}