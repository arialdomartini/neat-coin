using System;
using System.Collections.Generic;
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
        private readonly SHA256 _cryptography;
        private readonly ImmutableList<Transaction> _emptyTransactionList = ImmutableList.Create<Transaction>();
        private DateTimeOffset Now => DateTimeOffset.UtcNow;

        public BlockChainTest()
        {
            _sut = new BlockChain(new SHA256(), 2);
            _cryptography = new SHA256();
        }

        [Fact]
        public void blocks_can_be_found_given_their_hash()
        {
            var genesisBlock = _sut.Last;
            var block = Block.Create(_cryptography, DateTimeOffset.UtcNow, _emptyTransactionList, genesisBlock.Hash, 3, 50);
            _sut.Push(block);

            var result = _sut.GetBlockByHash(block.Hash);

            result.Should().Be(block);
        }

        [Fact]
        public void should_return_null_if_no_blocks_is_found()
        {
            var block = Block.Create(_cryptography, DateTimeOffset.UtcNow, _emptyTransactionList, "0", 3, 50);
            _sut.Push(block);

            var result = _sut.GetBlockByHash("hash of unknown block");

            result.Should().Be(null);
        }

        [Fact]
        public void can_contain_more_than_one_block()
        {
            var genesisBlock = _sut.Last;
            var block2 = Block.Create(_cryptography, Now, _emptyTransactionList, genesisBlock.Hash, 3, 50);
            var block3 = Block.Create(_cryptography, Now, _emptyTransactionList, block2.Hash, 3, 50);

            _sut.Push(block2);
            _sut.Push(block3);

            _sut.GetBlockByHash(block2.Hash).Should().BeEquivalentTo(block2);
            _sut.GetBlockByHash(block3.Hash).Should().BeEquivalentTo(block3);
        }

        [Fact]
        public void blocks_are_chained()
        {
            var genesisBlock = _sut.Last;
            var block2 = Block.Create(_cryptography, Now, _emptyTransactionList, genesisBlock.Hash, 3, 50);

            _sut.Push(block2);

            var lastBlock = _sut.Last;
            var parent = _sut.GetBlockByHash(lastBlock.Parent);
            parent.Hash.Should().Be(genesisBlock.Hash);
        }

        [Fact]
        public void unchained_blocks_cannot_be_added()
        {
            var block1 = Block.Create(_cryptography, Now, _emptyTransactionList, "0", 3, 50);
            var block2 = Block.Create(_cryptography, Now, _emptyTransactionList, "not existing parent", 3, 50);

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

            genesisBlock.IsValid.Should().Be(true);
        }
    }
}