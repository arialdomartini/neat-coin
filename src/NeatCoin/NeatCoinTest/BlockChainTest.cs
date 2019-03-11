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

        public BlockChainTest()
        {
            _sut = new BlockChain();
            _cryptography = new SHA256();
            _now = DateTimeOffset.UtcNow;
        }

        [Fact]
        public void can_host_a_block()
        {
            var block = new Block(_cryptography, _now, "some content");

            _sut.Push(block);
            var result = _sut.Latest;

            result.Should().Be(block);
        }

        [Fact]
        public void blocks_can_be_found_given_their_hash()
        {
            var block = new Block(_cryptography, _now, "some content");
            _sut.Push(block);

            var result = _sut.GetBlockByHash(block.Hash);

            result.Should().Be(block);
        }

        [Fact]
        public void should_return_null_if_no_blocks_is_found()
        {
            var block = new Block(_cryptography, _now, "some content");
            _sut.Push(block);

            var result = _sut.GetBlockByHash("hash of unknown block");

            result.Should().Be(null);
        }
    }
}