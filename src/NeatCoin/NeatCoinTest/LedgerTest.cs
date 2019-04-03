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
            var page1 = new Page(
                new Transaction("sender", "receiver", 1));
            var page2 = new Page(
                    new Transaction("sender", "receiver", 1))
                .LinkTo(page1);
            var page3 = new Page(
                    new Transaction("sender", "receiver", 1),
                    new Transaction("sender", "receiver", 2))
                .LinkTo(page2);

            var sut = new Ledger(page1, page2, page3);

            var result = sut.Balance("sender");

            result.Should().Be(- (1 + 2 + 1 + 1));
        }

        [Fact]
        public void can_return_receiver_s_balance()
        {
            var page1 = new Page(
                new Transaction("sender", "receiver", 1));
            var page2 = new Page(
                    new Transaction("sender", "receiver", 1))
                .LinkTo(page1);
            var page3 = new Page(
                    new Transaction("sender", "receiver", 1),
                    new Transaction("sender", "receiver", 2))
                .LinkTo(page2);

            var sut = new Ledger(page1, page2, page3);

            var result = sut.Balance("receiver");

            result.Should().Be(1 + 1 + 1 + 2);
        }

        [Fact]
        public void can_return_an_unknown_account_s_balance()
        {
            var page1 = new Page(
                new Transaction("sender", "receiver", 1));
            var page2 = new Page(
                    new Transaction("sender", "receiver", 1))
                .LinkTo(page1);
            var page3 = new Page(
                    new Transaction("sender", "receiver", 1),
                    new Transaction("sender", "receiver", 2))
                .LinkTo(page2);

            var sut = new Ledger(page1, page2, page3);

            var result = sut.Balance("unknown account");

            result.Should().Be(0);
        }
    }
}