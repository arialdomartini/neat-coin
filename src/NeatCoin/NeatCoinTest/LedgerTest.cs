using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        const int Difficulty = 2;

        [Fact]
        public void should_retrieve_sender_s_balance()
        {
            var sut =
                new Ledger(Difficulty)
                    .Append(new Page(new Transaction("A", "B", 10)))
                    .Append(new Page(new Transaction("B", "A", 2)));

            sut.Balance("A").Should().Be(-8);
        }

        [Fact]
        public void should_retrieve_receiver_s_balance()
        {
            var sut =
                new Ledger(Difficulty)
                    .Append(new Page(new Transaction("A", "B", 10)))
                    .Append(new Page(new Transaction("B", "A", 2)))
                    .Append(new Page(new Transaction("B", "A", 2)));

            sut.Balance("B").Should().Be(10 - 2 - 2);
        }

        [Fact]
        public void should_retrieve_unknown_account_s_balance()
        {
            var sut =
                new Ledger(Difficulty)
                    .Append(new Page(new Transaction("A", "B", 10)))
                    .Append(new Page(new Transaction("B", "A", 2)));

            sut.Balance("unknown account").Should().Be(0);
        }

        [Fact]
        public void ledger_is_not_valid_if_its_pages_are_not_valid()
        {
            var sut =
                new Ledger(
                    Difficulty,
                    new Pages(ImmutableList.Create(new Page(new Transaction("A", "B", 10)))));

            sut.IsValid.Should().Be(false);
        }

        [Fact]
        public void ledger_is_valid_if_its_pages_are_valid()
        {
            var sut =
                new Ledger(Difficulty)
                    .Append(new Page(new Transaction("A", "B", 10)))
                    .Append(new Page(new Transaction("B", "A", 2)));

            sut.IsValid.Should().Be(true);
        }
    }
}