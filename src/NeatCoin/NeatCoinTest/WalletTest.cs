using FluentAssertions;
using NeatCoin;
using Xunit;

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
        public void should_retrieve_sender_balance()
        {
            var wallet = _sut
                .Push(new Transaction("from", "to", 100));

            wallet.BalanceOf("from").Should().Be(-100);
            wallet.BalanceOf("to").Should().Be(100);
        }

        [Fact]
        public void should_retrieve_sender_balance_with_multiple_transactions()
        {
            var wallet = _sut
                .Push(new Transaction("from", "to", 100))
                .Push(new Transaction("from", "to", 100))
                .Push(new Transaction("to", "from", 50));

            wallet.BalanceOf("from").Should().Be(-150);
            wallet.BalanceOf("to").Should().Be(150);
        }
    }
}