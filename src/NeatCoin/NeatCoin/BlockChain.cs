namespace NeatCoin
{
    internal class BlockChain
    {
        private Block _block;

        internal void Push(Block block)
        {
            _block = block;
        }

        public Block Latest => _block;

        public Block GetBlockByHash(string blockHash) => Latest.Hash == blockHash ? Latest : null;
    }
}