using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;
using SHA256 = NeatCoin.Cryptography.SHA256;

namespace NeatCoinTest
{
    public class BlockTest
    {
        private readonly SHA256 _cryptography;
        private DateTimeOffset Now = DateTimeOffset.UtcNow;
        private readonly ImmutableList<Transaction> _emptyTransactionList = ImmutableList.Create<Transaction>();
        private readonly BlockChain _blockChain;
        private const int Difficulty = 2;

        public BlockTest()
        {
            _cryptography = new SHA256();
            _blockChain = new BlockChain(_cryptography, Difficulty, 50);
        }

        [Fact]
        public void blocks_created_in_different_moments_should_have_different_hash_values()
        {
            var now = Now;
            var block1 = _blockChain.Create(now, _emptyTransactionList, "0");
            var block2 = _blockChain.Create(now.AddMilliseconds(1), _emptyTransactionList, block1.Hash);

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void blocks_with_different_parents_should_have_different_hash_values()
        {
            var sameMoment = Now;
            var block1 = _blockChain.Create(sameMoment, _emptyTransactionList, "0");
            var block2 = _blockChain.Create(sameMoment, _emptyTransactionList, "1");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void blocks_with_different_transactions_should_have_different_hash_values()
        {
            var transactionList1 = new List<Transaction> { new Transaction("some sender", "some receiver", 100)}.ToImmutableList();
            var transactionList2 = new List<Transaction> { new Transaction("some sender", "some receiver", 100), new Transaction("some sender", "some receiver", 100)}.ToImmutableList();
            var block1 = _blockChain.Create(Now, transactionList1, "0");
            var block2 = _blockChain.Create(Now, transactionList2, "0");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void unmined_blocks_should_not_be_valid()
        {
            var sut = _blockChain.Create(Now, _emptyTransactionList, "0");

            var result = sut.IsValid;

            result.Should().Be(false);
        }

        [Fact]
        public void mined_blocks_should_be_valid()
        {
            var block = _blockChain
                .Create(Now, _emptyTransactionList, "0");

            var sut = _blockChain.Mine(block, "some miner");

            var result = sut.IsValid;

            result.Should().Be(true);
        }

        [Fact]
        public void unmined_blocks_should_contain_no_reward_transactions()
        {
            var sut = _blockChain
                .Create(Now, _emptyTransactionList, "0");

            sut.RewardTransaction.Should().Be(null);
        }

        [Fact]
        public void a_mined_block_should_contain_a_reward_transaction()
        {
            var block = _blockChain
                .Create(Now, _emptyTransactionList, "0");

            var sut = _blockChain.Mine(block, "some miner");

            var result = sut.RewardTransaction;
            result.Should().BeEquivalentTo(
                new Transaction("mint", "some miner", 50));
        }
    }
}