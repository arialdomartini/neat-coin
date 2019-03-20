using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;

namespace NeatCoin
{
    public struct Block
    {
        public ImmutableList<Transaction> Transactions { get; }
        public int Nonce { get; }

        public Hash Hash =>
            Convert.ToBase64String(
                SHA256(new{ Transactions, Nonce }.ToJson()));

        public Transaction RewardTransaction => Transactions.SingleOrDefault(t => t.IsReward);

        public Hash Parent;

        internal Block(ImmutableList<Transaction> transactions, Hash parent, int nonce)
        {
            Transactions = transactions;
            Nonce = nonce;
            Parent = parent;
        }

        private static byte[] SHA256(string @string) =>
            new SHA256Managed()
                .ComputeHash(@string.ToByteArray2());

        public bool HashMatchesDifficulty(int difficulty) =>
            Hash.StartsWithASeriesOf0Repeated(difficulty);
    }
}