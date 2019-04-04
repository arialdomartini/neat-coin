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

        private Transaction Transaction { get; }

        public int Balance(string account)
        {
            if(account == Transaction.Sender)
                return -Transaction.Amount;
            if(account == Transaction.Receiver)
                return Transaction.Amount;
            return 0;
        }
    }
}