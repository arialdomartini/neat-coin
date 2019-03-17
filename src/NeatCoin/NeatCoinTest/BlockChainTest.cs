using System;
using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using NeatCoin.Cryptography;
using Xunit;

namespace NeatCoinTest
{
    public class BlockChainTest
    {
        private readonly BlockChain _sut;
        private readonly ImmutableList<Transaction> _emptyTransactionList = ImmutableList.Create<Transaction>();
        private DateTimeOffset Now => DateTimeOffset.UtcNow;

        public BlockChainTest()
        {
            _sut = new BlockChain(new SHA256(), 2, 50);
        }

        [Fact]
        public void blocks_can_be_found_given_their_hash()
        {
            var genesisBlock = _sut.Last;
            var block = _sut.Create(DateTimeOffset.UtcNow, _emptyTransactionList, genesisBlock.Hash);
            _sut.Push(block);

            var result = _sut.GetBlockByHash(block.Hash);

            result.Should().Be(block);
        }

        [Fact]
        public void should_return_null_if_no_blocks_is_found()
        {
            var block = _sut.Create(DateTimeOffset.UtcNow, _emptyTransactionList, "0");
            _sut.Push(block);

            var result = _sut.GetBlockByHash("hash of unknown block");

            result.Should().Be(null);
        }

        [Fact]
        public void can_contain_more_than_one_block()
        {
            var genesisBlock = _sut.Last;
            var block2 = _sut.Create(Now, _emptyTransactionList, genesisBlock.Hash);
            var block3 = _sut.Create(Now, _emptyTransactionList, block2.Hash);

            _sut.Push(block2);
            _sut.Push(block3);

            _sut.GetBlockByHash(block2.Hash).Should().BeEquivalentTo(block2);
            _sut.GetBlockByHash(block3.Hash).Should().BeEquivalentTo(block3);
        }

        [Fact]
        public void blocks_are_chained()
        {
            var genesisBlock = _sut.Last;
            var block2 = _sut.Create(Now, _emptyTransactionList, genesisBlock.Hash);

            _sut.Push(block2);

            var lastBlock = _sut.Last;
            var parent = _sut.GetBlockByHash(lastBlock.Parent);
            parent.Hash.Should().Be(genesisBlock.Hash);
        }

        [Fact]
        public void unchained_blocks_cannot_be_added()
        {
            var block1 = _sut.Create(Now, _emptyTransactionList, "0");
            var block2 = _sut.Create(Now, _emptyTransactionList, "not existing parent");

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

        [Fact]
        public void genesis_block_should_be_valid()
        {
            var genesisBlock = _sut.Last;

            var result = _sut.IsValid(genesisBlock);

            result.Should().Be(true);
        }

        [Fact]
        public void unmined_blocks_should_not_be_valid()
        {
            var block = _sut.Create(Now, _emptyTransactionList, "0");

            var result = _sut.IsValid(block);

            result.Should().Be(false);
        }

        [Fact]
        public void mined_blocks_should_be_valid()
        {
            var block = _sut.Create(Now, _emptyTransactionList, "0");
            var sut = _sut.Mine(block, "some miner");

            var result = _sut.IsValid(sut);

            result.Should().Be(true);
        }
    }
}