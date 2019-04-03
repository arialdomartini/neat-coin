namespace NeatCoin
{
    public class Ledger
    {
        public Transaction Transaction { get; }

        public Ledger(Transaction transaction)
        {
            Transaction = transaction;
        }

        public int Balance(string account)
        {
            if (Transaction.Sender == account) return -Transaction.Amount;
            if(Transaction.Receiver == account) return Transaction.Amount;
            return 0;
        }
    }
}