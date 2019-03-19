using System;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace NeatCoin
{
    public struct Group
    {
        public ImmutableList<Transaction> Transactions { get; }
        public int Nonce { get; }

        public Hash Hash =>
            Convert.ToBase64String(
                SHA256(new{ Transactions, Nonce }.ToJson()));

        public Hash Parent;

        public Group(ImmutableList<Transaction> transactions, Hash parent, int nonce)
        {
            Transactions = transactions;
            Nonce = nonce;
            Parent = parent;
        }

        private static byte[] SHA256(string @string) =>
            new SHA256Managed()
                .ComputeHash(@string.ToByteArray());

        public bool IsValid(int difficulty) =>
            Hash.StartsWithASeriesOf0Repeated(difficulty);
    }
}