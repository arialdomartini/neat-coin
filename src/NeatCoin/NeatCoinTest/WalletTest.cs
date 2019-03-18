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

    }
}