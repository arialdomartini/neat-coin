using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;
using Group = NeatCoin.Group;

namespace NeatCoinTest
{
    public class WalletTest
    {
        private readonly Wallet _sut;

        private readonly Group _group1 = new Group(ImmutableList.Create(
            new Transaction("from", "to", 100),
            new Transaction("from", "to", 100)));

        private readonly Group _group2 = new Group(ImmutableList.Create(
            new Transaction("to", "from", 50)));

        public WalletTest()
        {
            _sut = new Wallet();
        }

        [Fact]
        public void should_retrieve_accounts_balances_with_multiple_transactions()
        {
            var group1 = new Group(ImmutableList.Create(
                new Transaction("from", "to", 100),
                new Transaction("from", "to", 100)));
            var group2 = new Group(ImmutableList.Create(
                new Transaction("to", "from", 50)));

            var wallet = _sut
                .Push(group1)
                .Push(group2);

            wallet.BalanceOf("from").Should().Be(-150);
            wallet.BalanceOf("to").Should().Be(150);
        }

        [Fact]
        public void should_retrieve_last_group()
        {
            var wallet = _sut
                .Push(_group1)
                .Push(_group2);

            wallet.Last.Should().Be(_group2);
        }

        [Fact]
        public void should_retrieve_groups_by_hash()
        {
            var wallet = _sut
                .Push(_group1)
                .Push(_group2);

            var result1 = wallet.GetGroup(_group1.Hash);
            var result2 = wallet.GetGroup(_group2.Hash);

            result1.Should().Be(_group1);
            result2.Should().Be(_group2);
        }
    }
}