namespace NeatCoin
{
    internal class Transaction
    {
        private const string Mint = "mint";
        internal string From { get; }
        internal string To { get; }
        internal int Amount { get; }

        public Transaction(string from, string to, int amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }

        internal bool IsAReward() =>  From== Mint;

        public static Transaction CreateReward(string miner, int rewardAmount) => 
            new Transaction(Mint, miner, rewardAmount);
    }
}