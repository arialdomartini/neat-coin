using System;

namespace NeatCoin
{
    public class Transaction
    {
        public Account Sender { get; }
        public Account Receiver { get; }
        public Amount Amount { get; }

        public Transaction(Account sender, Account receiver, Amount amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
        }

        public static Func<Transaction, bool> IsSender(Account sender) =>
            t => t.Sender == sender;

        public static Func<Transaction, bool> IsReceiver(Account account) =>
            t => t.Receiver == account;
    }
}