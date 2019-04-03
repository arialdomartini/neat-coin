using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void can_return_sender_s_balance()
        {
            var transaction1 = new Transaction("sender", "receiver", 10);
            var transaction2 = new Transaction("sender", "receiver", 100);
            var transactions = ImmutableList.Create(transaction1, transaction2);
            var sut = new Ledger(transactions);

            var result = sut.Balance("sender");

            result.Should().Be(-110);
        }

        [Fact]
        public void can_return_receiver_s_balance()
        {
            var transaction1 = new Transaction("sender", "receiver", 1);
            var transaction2 = new Transaction("sender", "receiver", 2);
            var transactions = ImmutableList.Create(transaction1, transaction2);
            var sut = new Ledger(transactions);

            var result = sut.Balance("receiver");

            result.Should().Be(3);
        }

        [Fact]
        public void can_return_an_unknown_account_s_balance()
        {
            var transaction1 = new Transaction("sender", "receiver", 10);
            var transaction2 = new Transaction("sender", "receiver", 10);
            var transactions = ImmutableList.Create(transaction1, transaction2);
            var sut = new Ledger(transactions);

            var result = sut.Balance("unknown account");

            result.Should().Be(0);
        }
    }
}