namespace NeatCoin
{
    public class Ledger
    {
        private Ledger(Transaction transaction)
        {
            Transaction = transaction;
        }

        public Ledger()
        {
            
        }

        public Ledger Append(Transaction transaction) => new Ledger(transaction);

        public Transaction Transaction { get; }
    }
}