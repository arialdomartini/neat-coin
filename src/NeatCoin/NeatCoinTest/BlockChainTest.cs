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
        private readonly DateTimeOffset _now;
        private DateTimeOffset Now = DateTimeOffset.UtcNow;

        public BlockChainTest()
        {
            _sut = new BlockChain();
            _cryptography = new SHA256();
            _now = Now;
        }

        [Fact]
        public void can_host_a_block()
        {
            var block = new Block(_cryptography, _now, "some content", "0");

            _sut.Push(block);
            var result = _sut.Last;

            result.Should().Be(block);
        }

        [Fact]
        public void blocks_can_be_found_given_their_hash()
        {
            var block = new Block(_cryptography, _now, "some content", "0");
            _sut.Push(block);

            var result = _sut.GetBlockByHash(block.Hash);

            result.Should().Be(block);
        }

        [Fact]
        public void should_return_null_if_no_blocks_is_found()
        {
            var block = new Block(_cryptography, _now, "some content", "0");
            _sut.Push(block);

            var result = _sut.GetBlockByHash("hash of unknown block");

            result.Should().Be(null);
        }

        [Fact]
        public void can_contain_more_than_one_block()
        {
            var block1 = new Block(_cryptography, Now, "some content", "0");
            var block2 = new Block(_cryptography, Now, "some content", block1.Hash);

            _sut.Push(block1);
            _sut.Push(block2);

            _sut.GetBlockByHash(block1.Hash).Should().BeEquivalentTo(block1);
            _sut.GetBlockByHash(block2.Hash).Should().BeEquivalentTo(block2);
        }

        [Fact]
        public void blocks_are_chained()
        {
            var block1 = new Block(_cryptography, Now, "some content", "0");
            var block2 = new Block(_cryptography, Now, "some content", block1.Hash);

            _sut.Push(block1);
            _sut.Push(block2);

            var lastBlock = _sut.Last;
            var parent = _sut.GetBlockByHash(lastBlock.Parent);
            parent.Hash.Should().Be(block1.Hash);
        }
    }
}