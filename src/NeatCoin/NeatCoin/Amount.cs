namespace NeatCoin
{
    public class Amount
    {
        private Amount(int value)
        {
            Value = value;
        }

        public static implicit operator int(Amount amount) =>
            amount.Value;

        public static implicit operator Amount(int amount) =>
            new Amount(amount);
        
        private int Value { get; }
    }
}