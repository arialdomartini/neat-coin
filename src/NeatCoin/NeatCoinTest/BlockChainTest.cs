using System;
using FluentAssertions;
using NeatCoin;
using NeatCoin.Cryptography;
using Xunit;

namespace NeatCoinTest
{
    public class BlockChainTest
    {
        private readonly BlockChain _sut;
        private readonly SHA256 _cryptography;
        private DateTimeOffset Now => DateTimeOffset.UtcNow;

        public BlockChainTest()
        {
            _sut = new BlockChain(new SHA256());
            _cryptography = new SHA256();
        }

        [Fact]
        public void blocks_can_be_found_given_their_hash()
        {
            var genesisBlock = _sut.Last;
            var block = new Block(_cryptography, DateTimeOffset.UtcNow, "some content", genesisBlock.Hash);
            _sut.Push(block);

            var result = _sut.GetBlockByHash(block.Hash);

            result.Should().Be(block);
        }

        [Fact]
        public void should_return_null_if_no_blocks_is_found()
        {
            var block = new Block(_cryptography, DateTimeOffset.UtcNow, "some content", "0");
            _sut.Push(block);

            var result = _sut.GetBlockByHash("hash of unknown block");

            result.Should().Be(null);
        }

        [Fact]
        public void can_contain_more_than_one_block()
        {
            var genesisBlock = _sut.Last;
            var block2 = new Block(_cryptography, Now, "some content", genesisBlock.Hash);
            var block3 = new Block(_cryptography, Now, "some content", block2.Hash);

            _sut.Push(block2);
            _sut.Push(block3);

            _sut.GetBlockByHash(block2.Hash).Should().BeEquivalentTo(block2);
            _sut.GetBlockByHash(block3.Hash).Should().BeEquivalentTo(block3);
        }

        [Fact]
        public void blocks_are_chained()
        {
            var genesisBlock = _sut.Last;
            var block2 = new Block(_cryptography, Now, "some content", genesisBlock.Hash);

            _sut.Push(block2);

            var lastBlock = _sut.Last;
            var parent = _sut.GetBlockByHash(lastBlock.Parent);
            parent.Hash.Should().Be(genesisBlock.Hash);
        }

        [Fact]
        public void unchained_blocks_cannot_be_added()
        {
            var block1 = new Block(_cryptography, Now, "some content", "0");
            var block2 = new Block(_cryptography, Now, "some content", "not existing parent");

            _sut.Push(block1);
            _sut.Push(block2);

            var result = _sut.GetBlockByHash(block2.Hash);
            result.Should().BeNull();
        }

        [Fact]
        public void genesis_block_is_always_present()
        {
            _sut.Last.Parent.Should().Be("0");
        }
    }
}