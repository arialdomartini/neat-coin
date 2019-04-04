using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        private readonly int Reward = 50;
        const int Difficulty = 2;

        [Fact]
        public void should_retrieve_sender_s_balance()
        {
            var sut =
                new Ledger(Difficulty, 50)
                    .Append(new Page(new Transaction("A", "B", 10)), "author")
                    .Append(new Page(new Transaction("B", "A", 2)), "author");

            sut.Balance("A").Should().Be(-8);
        }

        [Fact]
        public void should_retrieve_receiver_s_balance()
        {
            var sut =
                new Ledger(Difficulty, 50)
                    .Append(new Page(new Transaction("A", "B", 10)), "author")
                    .Append(new Page(new Transaction("B", "A", 2)), "author")
                    .Append(new Page(new Transaction("B", "A", 2)), "author");

            sut.Balance("B").Should().Be(10 - 2 - 2);
        }

        [Fact]
        public void should_retrieve_unknown_account_s_balance()
        {
            var sut =
                new Ledger(Difficulty, 50)
                    .Append(new Page(new Transaction("A", "B", 10)), "author")
                    .Append(new Page(new Transaction("B", "A", 2)), "author");

            sut.Balance("unknown account").Should().Be(0);
        }

        [Fact]
        public void ledger_is_not_valid_if_its_pages_are_not_valid()
        {
            var sut =
                new Ledger(
                    Difficulty, 50, new Pages(ImmutableList.Create(new Page(new Transaction("A", "B", 10)))));

            sut.IsValid.Should().Be(false);
        }

        [Fact]
        public void ledger_is_valid_if_its_pages_are_valid()
        {
            var sut =
                new Ledger(Difficulty, 50)
                    .Append(new Page(new Transaction("A", "B", 10)), "author")
                    .Append(new Page(new Transaction("B", "A", 2)), "author");

            sut.IsValid.Should().Be(true);
        }

        [Fact]
        public void valid_pages_contain_a_reward_transaction()
        {
            var sut =
                new Ledger(Difficulty, 50)
                    .Append(new Page(new Transaction("from", "to", 100)), "author");

            sut.Balance("author").Should().Be(Reward);
        }

    }
}