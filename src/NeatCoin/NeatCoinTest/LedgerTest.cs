using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void should_retrieve_sender_s_balance()
        {
            var sut =
                new Ledger()
                    .Append(new Transaction("A", "B", 10))
                    .Append(new Transaction("B", "A", 2));

            sut.Balance("A").Should().Be(-8);
        }

        [Fact]
        public void should_retrieve_receiver_s_balance()
        {
            var sut =
                new Ledger()
                    .Append(new Transaction("A", "B", 10))
                    .Append(new Transaction("B", "A", 2));

            sut.Balance("B").Should().Be(8);
        }

        [Fact]
        public void should_retrieve_unknown_account_s_balance()
        {
            var sut =
                new Ledger()
                    .Append(new Transaction("A", "B", 10))
                    .Append(new Transaction("B", "A", 2));

            sut.Balance("unknown account").Should().Be(0);
        }
    }
}