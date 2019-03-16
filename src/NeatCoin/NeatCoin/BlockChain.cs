using System.Collections.Generic;
using System.Linq;
using NeatCoin.Cryptography;

namespace NeatCoin
{
    internal class BlockChain
    {
        private readonly List<Block> _blocks = new List<Block>();
        private readonly ICryptography _cryptography;
        private readonly int _difficulty;
        private readonly int _rewardAmount;

        public BlockChain(ICryptography cryptography, int difficulty, int rewardAmount)
        {
            _cryptography = cryptography;
            _difficulty = difficulty;
            _rewardAmount = rewardAmount;

            var genesisBlock = CreateGenesisBlock(_cryptography, _difficulty);
            _blocks.Add(genesisBlock);
        }

        private Block CreateGenesisBlock(ICryptography cryptography, int difficulty)
        {
            return Mine(
                new GenesisBlock(cryptography, difficulty),
                "some miner");
        }

        internal void Push(Block block)
        {
            if ( block.IsChainedTo(Last))
                _blocks.Add(block);
        }

        public Block Last => _blocks.LastOrDefault();

        public Block GetBlockByHash(string blockHash) =>
            _blocks.SingleOrDefault(b => b.Hash == blockHash);

        public Block Mine(Block block, string miner)
        {
            var rewardTransaction = Transaction.CreateReward(miner, _rewardAmount);
            for (var nonce = 0; nonce < int.MaxValue; nonce++)
            {
                var mined = block.CloneWithNonce(nonce, rewardTransaction, _rewardAmount);
                if (mined.IsValid)
                    return mined;
            }

            return null;
        }
    }
}