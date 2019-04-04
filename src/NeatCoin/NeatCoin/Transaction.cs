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

        public int BalanceOf(string account) =>
            (account == Receiver ? Amount : 0) -
            (account == Sender ? Amount : 0);

        public bool IsARewardTransaction => Sender == "mint";
    }
}