using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class GroupTest
    {
        private readonly Wallet _wallet;
        private ImmutableList<Group> _emptyList = ImmutableList.Create<Group>();
        private const int Difficulty = 2;

        public GroupTest()
        {
            _wallet = new Wallet(_emptyList, Difficulty);
        }

        [Fact]
        public void groups_with_different_transactions_should_have_different_hash_values()
        {
            var group1 = _wallet.MakeGroup(ImmutableList.Create(
                new Transaction("from", "to", 100),
                new Transaction("from", "to", 100)));
            var group2 = _wallet.MakeGroup(ImmutableList.Create(
                new Transaction("to", "from", 50)));

            var hash1 = group1.Hash;
            var hash2 = group2.Hash;

            hash1.Should().NotBe(hash2);
        }[Fact]

        public void groups_with_different_nonce_should_have_different_hash_values()
        {
            var transactionList = ImmutableList.Create(
                new Transaction("from", "to", 100),
                new Transaction("from", "to", 100));

            var group1 = _wallet.MakeGroup(transactionList, 1);
            var group2 = _wallet.MakeGroup(transactionList, 2);

            var hash1 = group1.Hash;
            var hash2 = group2.Hash;

            hash1.Should().NotBe(hash2);
        }
    }
}