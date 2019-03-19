using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class GroupTest
    {
        private readonly Wallet _wallet;

        public GroupTest()
        {
            _wallet = new Wallet();
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
        }
    }
}