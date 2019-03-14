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
        private readonly DateTimeOffset _createdAt;
        private readonly ImmutableList<Transaction> _transactions;
        public string Parent { get; }
        private readonly ICryptography _cryptography;
        private readonly int _difficulty;
        private int Nonce { get; }
        private readonly int _rewardAmount;

        protected Block(ICryptography cryptography, DateTimeOffset createdAt, ImmutableList<Transaction> transactions, string parent, int difficulty, int nonce, int rewardAmount)
        {
            _cryptography = cryptography;

            _createdAt = createdAt;
            _transactions = transactions;
            Parent = parent;
            _difficulty = difficulty;
            Nonce = nonce;
            _rewardAmount = rewardAmount;
        }



        public static Block Create(ICryptography cryptography, DateTimeOffset utcNow,
            ImmutableList<Transaction> emptyTransactionList, string parent, int difficulty, int reward, int nonce = 0) =>
            new Block(cryptography, utcNow, emptyTransactionList, parent, difficulty, nonce, reward);

        public string Hash => _cryptography.HashOf(Serialized);

        private string Serialized =>
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
        public Transaction RewardTransaction =>
            _transactions.FirstOrDefault(t => t.IsAReward());


        private bool MatchesDifficulty(int difficulty) =>
            Hash.StartsWith(new string('0', difficulty));

        public Block Mine(string miner)
        {
            var rewardTransaction = Transaction.CreateReward(miner, _rewardAmount);
            for (var nonce = 0; nonce < int.MaxValue; nonce++)
            {
                var block = CloneWithNonce(nonce, rewardTransaction, _rewardAmount);
                if (block.IsValid)
                    return block;
            }

            return null;
        }

        private Block CloneWithNonce(int nonce, Transaction rewardTransaction, int rewardAmount) =>
            Create(
                _cryptography,
                _createdAt,
                _transactions.Add(rewardTransaction),
                Parent,
                _difficulty,
                rewardAmount,
                nonce);
    }
}