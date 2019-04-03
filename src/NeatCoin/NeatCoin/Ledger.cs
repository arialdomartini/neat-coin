namespace NeatCoin
{
    public class Ledger
    {
        public Transaction Transaction { get; }

        public Ledger(Transaction transaction)
        {
            Transaction = transaction;
        }
    }
}