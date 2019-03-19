using System;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class BlockChain
    {
        private readonly int _difficulty;
        private readonly ImmutableList<Block> _blocks;
        private readonly int _rewardAmount;
        private const string Mint = "mint";

        public BlockChain(ImmutableList<Block> blocks, int difficulty, int rewardAmount)
        {
            _blocks = blocks;
            _difficulty = difficulty;
            _rewardAmount = rewardAmount;
        }

        public Block Last => _blocks.Last();
        public bool IsValid => _blocks.All(g => g.HashMatchesDifficulty(_difficulty));

        public BlockChain Push(Block block) =>
            new BlockChain(_blocks.Add(block), _difficulty, _rewardAmount);

        private Hash LastHash()
        {
            var lastHash = default(Hash);
            if(_blocks.Any())
                lastHash = Last.Hash;
            return lastHash;
        }

        public int BalanceOf(Account account) =>
            Total(Transaction.IsReceiver(account)) - Total(Transaction.IsSender(account));

        private int Total(Func<Transaction, bool> condition) =>
            _blocks
                .SelectMany(g => g.Transactions)
                .Where(condition)
                .Sum(t => t.Amount);

        public Block GetBlock(string hash) =>
            _blocks.Find(g => g.Hash == hash);

        public Block MakeBlock(ImmutableList<Transaction> transactionList, int nonce = 0) =>
            new Block(transactionList, LastHash(), nonce);

        public Block Mine(Block block, string miner)
        {
            var rewardTransaction = new Transaction(Mint, miner, _rewardAmount);
            for (var i = 0; i < int.MaxValue; i++)
            {
                var cloneWithNonce = CloneWithNonce(block, i, rewardTransaction);
                if (cloneWithNonce.HashMatchesDifficulty(_difficulty))
                    return cloneWithNonce;
            }
            throw new Exception();
        }

        private Block CloneWithNonce(Block block, int nonce, Transaction rewardTransaction) =>
            new Block(block.Transactions.Add(rewardTransaction), block.Parent, nonce);
    }
}