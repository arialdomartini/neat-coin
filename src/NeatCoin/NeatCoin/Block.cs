using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using NeatCoin.Cryptography;
using Newtonsoft.Json;

namespace NeatCoin
{
    internal class Block
    {
        internal DateTimeOffset CreatedAt { get; }
        internal ImmutableList<Transaction> Transactions { get; }
        public string Parent { get; }
        private readonly ICryptography _cryptography;
        private readonly int _difficulty;
        private int Nonce { get; }

        internal Block(ICryptography cryptography, DateTimeOffset createdAt, ImmutableList<Transaction> transactions, string parent, int difficulty, int nonce)
        {
            _cryptography = cryptography;

            CreatedAt = createdAt;
            Transactions = transactions;
            Parent = parent;
            _difficulty = difficulty;
            Nonce = nonce;
        }

        public string Hash => _cryptography.HashOf(Serialized);

        private string Serialized =>
            JsonConvert.SerializeObject(
                new
                {
                    CreatedAt = CreatedAt.AsString(),
                    Content = Transactions,
                    Parent,
                    Nonce
                });

        public bool IsChainedTo(Block last) => Parent == last.Hash;

        public bool IsValid => MatchesDifficulty(_difficulty);
        public Transaction RewardTransaction =>
            Transactions.FirstOrDefault(t => t.IsAReward());

        private bool MatchesDifficulty(int difficulty) =>
            Hash.StartsWith(new string('0', difficulty));
    }
}