namespace NeatCoinTest
{
    public struct Account
    {
        private string Value { get; }

        private Account(string value)
        {
            Value = value;
        }

        public static implicit operator string(Account account) =>
            account.Value;

        public static implicit operator Account(string account) =>
            new Account(account);
    }
}