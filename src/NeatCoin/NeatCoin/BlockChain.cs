using System.Collections.Generic;
using System.Linq;

namespace NeatCoin
{
    internal class BlockChain
    {
        private readonly List<Block> _blocks = new List<Block>();

        internal void Push(Block block)
        {
            _blocks.Add(block);
        }

        public Block Last => _blocks.LastOrDefault();

        public Block GetBlockByHash(string blockHash) =>
            _blocks.SingleOrDefault(b => b.Hash == blockHash);
    }
}