using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void should_contain_a_transaction()
        {
            var ledger = new Ledger()
                .Append(
                    new Transaction("sender", "receiver", 100));

            ledger.Transaction.Should().Be(new Transaction("sender", "receiver", 100));
        }

        [Fact]
        public void should_calculate_sender_s_balance()
        {
            var ledger = new Ledger()
                .Append(
                    new Transaction("sender", "receiver", 100));

            ledger.BalanceOf("sender").Should().Be(-100);
        }

        [Fact]
        public void should_calculate_receiver_s_balance()
        {
            var ledger = new Ledger()
                .Append(
                    new Transaction("sender", "receiver", 100));

            ledger.BalanceOf("receiver").Should().Be(100);
        }

        [Fact]
        public void unknown_account_s_balance_should_be_0()
        {
            var ledger = new Ledger()
                .Append(
                    new Transaction("sender", "receiver", 100));

            ledger.BalanceOf("unknown account").Should().Be(0);
        }
    }
}