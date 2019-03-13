using System;
using System.Collections.Generic;
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

        protected Block(ICryptography cryptography, DateTimeOffset createdAt, List<Transaction> transactions, string parent)
        {
            _cryptography = cryptography;

            _createdAt = createdAt;
            _transactions = transactions;
            Parent = parent;
        }

        public static Block Create(SHA256 cryptography, DateTimeOffset utcNow, List<Transaction> emptyTransactionList, string parent) =>
            new Block(cryptography, utcNow, emptyTransactionList, parent);

        public string Hash => _cryptography.HashOf(Serialized);

        public string Serialized =>
            JsonConvert.SerializeObject(
                new
                {
                    CreatedAt = _createdAt.AsString(),
                    Content = _transactions,
                    Parent
                });

        public bool IsChainedTo(Block last) => Parent == last.Hash;

        public bool IsValid(string hash) => Hash == hash;
    }
}