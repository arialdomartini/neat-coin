namespace NeatCoin
{
    public class RewardTransaction : Transaction
    {
        public override bool IsReward => true;

        public RewardTransaction(Account sender, Account receiver, Amount amount) : base(sender, receiver, amount)
        {
        }
    }
}