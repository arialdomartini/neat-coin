using FluentAssertions;
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
    }
}