using System;
using System.Collections.Generic;
using FluentAssertions;
using NeatCoin;
using Xunit;
using SHA256 = NeatCoin.Cryptography.SHA256;

namespace NeatCoinTest
{
    public class BlockTest
    {
        private readonly SHA256 _sha256;
        private DateTimeOffset Now = DateTimeOffset.UtcNow;
        private readonly List<Transaction> _emptyTransactionList = new List<Transaction>();

        public BlockTest()
        {
            _sha256 = new SHA256();
        }

        [Fact]
        public void blocks_created_in_different_moments_should_have_different_hash_values()
        {
            var now = Now;
            var block1 = Block.Create(_sha256, now, _emptyTransactionList, "0");
            var block2 = Block.Create(_sha256, now.AddMilliseconds(1), _emptyTransactionList, block1.Hash);

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void blocks_with_different_parents_should_have_different_hash_values()
        {
            var sameMoment = Now;
            var block1 = Block.Create(_sha256, sameMoment, _emptyTransactionList, "0");
            var block2 = Block.Create(_sha256, sameMoment, _emptyTransactionList, "1");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void blocks_with_different_transactions_should_have_different_hash_values()
        {
            var transactionList1 = new List<Transaction> { new Transaction()};
            var transactionList2 = new List<Transaction> { new Transaction(), new Transaction()};
            var block1 = Block.Create(_sha256, Now, transactionList1, "0");
            var block2 = Block.Create(_sha256, Now, transactionList2, "0");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void should_be_valid_if_hash_matches()
        {
            var sut = Block.Create(_sha256, Now, _emptyTransactionList, "some parent");

            var result = sut.IsValid(_sha256.HashOf(sut.Serialized));

            result.Should().Be(true);
        }

        [Fact]
        public void should_not_be_valid_if_hash_does_not_match()
        {
            var sut = Block.Create(_sha256, Now, _emptyTransactionList, "some parent");

            var result = sut.IsValid("another hash");

            result.Should().Be(false);
        }
    }
}