namespace NeatCoin
{
    public struct Hash
    {
        private Hash(string value)
        {
            Value = value;
        }

        public static implicit operator string(Hash amount) =>
            amount.Value;

        public static implicit operator Hash(string amount) =>
            new Hash(amount);

        private string Value { get; }

        public bool StartsWithASeriesOf0Repeated(int times) => 
            Value.StartsWith(new string('0', times));
    }
}