using System.Collections.Generic;
using System.Linq;
using NeatCoin.Cryptography;

namespace NeatCoin
{
    internal class BlockChain
    {
        private readonly List<Block> _blocks = new List<Block>();

        public BlockChain(ICryptography cryptography, int difficulty)
        {
            var genesisBlock = CreateGenesisBlock(cryptography, difficulty);
            _blocks.Add(genesisBlock);
        }

        private static Block CreateGenesisBlock(ICryptography cryptography, int difficulty) =>
            new GenesisBlock(cryptography, difficulty)
                .Mine();

        internal void Push(Block block)
        {
            if (AddingTheGenesis(block) || block.IsChainedTo(Last))
                _blocks.Add(block);
        }

        private bool AddingTheGenesis(Block block)
        {
            return block.Parent == "0" && Last == null;
        }

        public Block Last => _blocks.LastOrDefault();

        public Block GetBlockByHash(string blockHash) =>
            _blocks.SingleOrDefault(b => b.Hash == blockHash);
    }
}