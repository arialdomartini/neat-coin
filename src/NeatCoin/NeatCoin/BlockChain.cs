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

        public Block Latest => _blocks.LastOrDefault();

        public Block GetBlockByHash(string blockHash) => Latest.Hash == blockHash ? Latest : null;
    }
}