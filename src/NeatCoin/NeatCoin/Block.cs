using System;
using System.Collections.Generic;
using System.Linq;
using NeatCoin.Cryptography;
using Newtonsoft.Json;

namespace NeatCoin
{
    internal class Block
    {
        private readonly DateTimeOffset _createdAt;
        private readonly List<Transaction> _transactions;
        public string Parent { get; }
        private readonly ICryptography _cryptography;
        private readonly int _difficulty;
        private int Nonce { get; }

        protected Block(ICryptography cryptography, DateTimeOffset createdAt, List<Transaction> transactions, string parent, int difficulty, int nonce)
        {
            _cryptography = cryptography;

            _createdAt = createdAt;
            _transactions = transactions;
            Parent = parent;
            _difficulty = difficulty;
            Nonce = nonce;
        }

        public static Block Create(ICryptography cryptography, DateTimeOffset utcNow,
            List<Transaction> emptyTransactionList, string parent, int difficulty, int nonce = 0) =>
            new Block(cryptography, utcNow, emptyTransactionList, parent, difficulty, nonce);

        public string Hash => _cryptography.HashOf(Serialized);

        public string Serialized =>
            JsonConvert.SerializeObject(
                new
                {
                    CreatedAt = _createdAt.AsString(),
                    Content = _transactions,
                    Parent,
                    Nonce
                });

        public bool IsChainedTo(Block last) => Parent == last.Hash;

        public bool IsValid => MatchesDifficulty(_difficulty);

        private bool MatchesDifficulty(int difficulty) =>
            Hash.StartsWith(new string('0', difficulty));

        public Block Mine()
        {
            for (var nonce = 0; nonce < int.MaxValue; nonce++)
            {
                var block = CloneWithNonce(nonce);
                if (block.IsValid)
                    return block;
            }

            return null;
        }

        private Block CloneWithNonce(int nonce) =>
            Create(_cryptography, _createdAt, _transactions, Parent, _difficulty, nonce);
    }
}