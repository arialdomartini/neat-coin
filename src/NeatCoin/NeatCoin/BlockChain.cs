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

        public BlockChain(ICryptography cryptography, int difficulty)
        {
            _cryptography = cryptography;
            _difficulty = difficulty;
            
            var genesisBlock = CreateGenesisBlock(_cryptography, _difficulty);
            _blocks.Add(genesisBlock);
        }

        private static Block CreateGenesisBlock(ICryptography cryptography, int difficulty) =>
            new GenesisBlock(cryptography, difficulty)
                .Mine("some miner");

        internal void Push(Block block)
        {
            if ( block.IsChainedTo(Last))
                _blocks.Add(block);
        }

        public Block Last => _blocks.LastOrDefault();

        public Block GetBlockByHash(string blockHash) =>
            _blocks.SingleOrDefault(b => b.Hash == blockHash);
    }
}