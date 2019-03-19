using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class BlockTest
    {
        private readonly BlockChain _wallet;
        private ImmutableList<Block> _emptyList = ImmutableList.Create<Block>();
        private const int Difficulty = 2;

        public BlockTest()
        {
            _wallet = new BlockChain(_emptyList, Difficulty, 50);
        }

        [Fact]
        public void blocks_with_different_transactions_should_have_different_hash_values()
        {
            var block1 = _wallet.MakeBlock(ImmutableList.Create(
                new Transaction("from", "to", 100),
                new Transaction("from", "to", 100)));
            var block2 = _wallet.MakeBlock(ImmutableList.Create(
                new Transaction("to", "from", 50)));

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }[Fact]

        public void blocks_with_different_nonce_should_have_different_hash_values()
        {
            var transactionList = ImmutableList.Create(
                new Transaction("from", "to", 100),
                new Transaction("from", "to", 100));

            var block1 = _wallet.MakeBlock(transactionList, 1);
            var block2 = _wallet.MakeBlock(transactionList, 2);

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }
    }
}