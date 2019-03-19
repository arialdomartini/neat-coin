using System;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace NeatCoin
{
    public struct Group
    {
        public ImmutableList<Transaction> Transactions { get; }
        public Hash Hash =>
            Convert.ToBase64String(
                SHA256(Transactions.ToJson()));

        public Hash Parent;

        public Group(ImmutableList<Transaction> transactions)
        {
            Transactions = transactions;
            Parent = null;
        }

        private static byte[] SHA256(string @string) =>
            new SHA256Managed()
                .ComputeHash(@string.ToByteArray());
    }
}