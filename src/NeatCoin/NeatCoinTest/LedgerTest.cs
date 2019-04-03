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
            var page = new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1)))
                .Append(new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1))))
                .Append(new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1),
                    new Transaction("sender", "receiver", 2))));

            var sut = new Ledger(page);

            var result = sut.Balance("sender");

            result.Should().Be(- (1 + 2 + 1 + 1));
        }

        [Fact]
        public void can_return_receiver_s_balance()
        {
            var page = new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1)))
                .Append(new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1))))
                .Append(new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1),
                    new Transaction("sender", "receiver", 2))));

            var sut = new Ledger(page);

            var result = sut.Balance("receiver");

            result.Should().Be(1 + 1 + 1 + 2);
        }

        [Fact]
        public void can_return_an_unknown_account_s_balance()
        {
            var page = new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1)))
                .Append(new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1))))
                .Append(new Page(ImmutableList.Create(
                    new Transaction("sender", "receiver", 1),
                    new Transaction("sender", "receiver", 2))));

            var sut = new Ledger(page);

            var result = sut.Balance("unknown account");

            result.Should().Be(0);
        }
    }
}