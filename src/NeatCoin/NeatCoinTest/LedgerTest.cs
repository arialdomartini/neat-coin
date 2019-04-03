using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void can_host_one_transaction()
        {
            var sut = new Ledger(new Transaction("sender", "receiver", 10));

            var result = sut.Transaction;

            result.Should().Be(new Transaction("sender", "receiver", 10));
        }

        [Fact]
        public void can_return_sender_s_balance()
        {
            var sut = new Ledger(new Transaction("sender", "receiver", 10));

            var result = sut.Balance("sender");

            result.Should().Be(-10);
        }

        [Fact]
        public void can_return_receiver_s_balance()
        {
            var sut = new Ledger(new Transaction("sender", "receiver", 10));

            var result = sut.Balance("receiver");

            result.Should().Be(10);
        }

        [Fact]
        public void can_return_an_unknown_account_s_balance()
        {
            var sut = new Ledger(new Transaction("sender", "receiver", 10));

            var result = sut.Balance("unknown account");

            result.Should().Be(0);
        }
    }
}