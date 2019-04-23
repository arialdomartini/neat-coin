using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void should_calculate_sender_s_balance()
        {
            var ledger = new Ledger()
                .Append(Page.Empty
                    .Append(new Transaction("A", "B", 100))
                    .Append(new Transaction("B", "A", 20)))
                .Append(Page.Empty
                    .Append(new Transaction("B", "A", 10)));

            ledger.BalanceOf("A").Should().Be(-70);
        }

        [Fact]
        public void should_calculate_receiver_s_balance()
        {
            var ledger = new Ledger()
                .Append(Page.Empty
                    .Append(new Transaction("A", "B", 100))
                    .Append(new Transaction("B", "A", 20)))
                .Append(Page.Empty
                    .Append(new Transaction("B", "A", 10)));

            ledger.BalanceOf("B").Should().Be(70);
        }

        [Fact]
        public void unknown_account_s_balance_should_be_0()
        {
            var ledger = new Ledger()
                .Append(Page.Empty
                    .Append(new Transaction("A", "B", 100))
                    .Append(new Transaction("B", "A", 20)))
                .Append(Page.Empty
                    .Append(new Transaction("B", "A", 10)));

            ledger.BalanceOf("unknown account").Should().Be(0);
        }
    }
}