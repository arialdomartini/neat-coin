namespace NeatCoin
{
    public struct Transaction
    {
        public string Sender { get; }
        public string Receiver { get; }
        public int Amount { get; }

        public Transaction(string sender, string receiver, int amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
        }
    }
}