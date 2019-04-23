namespace NeatCoin
{
    public struct Ledger
    {
        private Ledger(Transaction transaction)
        {
            Transaction = transaction;
        }

        public Ledger Append(Transaction transaction) =>
            new Ledger(transaction);

        public Transaction Transaction { get; }

        public int BalanceOf(string account)
        {
            if (account == Transaction.Receiver)
                return Transaction.Amount;
            if (account == Transaction.Sender)
                return -Transaction.Amount;
            return 0;
        }
    }
}