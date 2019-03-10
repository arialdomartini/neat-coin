namespace NeatCoin
{
    internal class BlockChain
    {
        private Block _block;

        internal void Push(Block block)
        {
            _block = block;
        }

        public Block GetLatest() => _block;

        public Block GetBlockByHash(string blockHash) => GetLatest().Hash == blockHash ? GetLatest() : null;
    }
}