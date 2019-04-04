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
                    .Append(
                        new Transaction("from", "to", 10));

            sut.Balance("from").Should().Be(-10);
        }

        [Fact]
        public void should_retrieve_receiver_s_balance()
        {
            var sut =
                new Ledger()
                    .Append(
                        new Transaction("from", "to", 10));

            sut.Balance("to").Should().Be(10);
        }

        [Fact]
        public void should_retrieve_unknown_account_s_balance()
        {
            var sut =
                new Ledger()
                    .Append(
                        new Transaction("from", "to", 10));

            sut.Balance("unknown account").Should().Be(0);
        }
    }
}