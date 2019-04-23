namespace NeatCoin
{
    public struct Ledger
    {
        private Ledger(Transaction transaction)
        {
            Transaction = transaction;
        }

        public Ledger Append(Transaction transaction)
        {
            return new Ledger(transaction);
        }

        public Transaction Transaction { get; }
    }
}