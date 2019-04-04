using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class PageTest
    {
        private readonly int Reward = 50;

        [Fact]
        public void pages_with_different_transactions_should_have_different_hash_values()
        {
            var page1 = new Page(new Transaction("from", "to", 10));
            var page2 = new Page(new Transaction("from", "to", 20));

            var hash1 = page1.Hash;
            var hash2 = page2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void pages_with_different_parent_should_have_different_hash_values()
        {
            var page1 = new Page("some parent", ImmutableList.Create(new Transaction("from", "to", 10)), "author");
            var page2 = new Page("another parent", ImmutableList.Create(new Transaction("from", "to", 10)), "author");

            var hash1 = page1.Hash;
            var hash2 = page2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void pages_with_different_nonces_should_have_different_hash_values()
        {
            var page1 = new Page("some parent", ImmutableList<Transaction>.Empty, 1, "author");
            var page2 = new Page("some parent", ImmutableList<Transaction>.Empty, 2, "author");

            var hash1 = page1.Hash;
            var hash2 = page2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void pages_with_different_authors_should_have_different_hash_values()
        {
            var page1 = new Page("some parent", ImmutableList<Transaction>.Empty, 1, "author");
            var page2 = new Page("some parent", ImmutableList<Transaction>.Empty, 1, "another author");

            var hash1 = page1.Hash;
            var hash2 = page2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void hashes_not_matching_the_difficulty_are_not_valid()
        {
            var sut = new Page(new Transaction("from", "to", 42));

            sut.HasAValidHash(2).Should().Be(false);
            sut.Hash.Should().NotStartWith("00");
        }

        [Fact]
        public void a_valid_hash_must_match_the_difficulty()
        {
            var page =
                new Page(
                        new Transaction("from", "to", 42))
                    .MakeTheHashValid(2);

            page.HasAValidHash(2).Should().Be(true);
            page.Hash.Should().StartWith("00");
        }

        [Fact]
        public void pages_need_to_contain_a_reward_transaction_to_be_valid()
        {
            var pageWithReward =
                new Page(
                        new Transaction("from", "to", 42),
                        new Transaction("mint", "author", Reward))
                    .MakeTheHashValid(2);

            var pageWithoutReward =
                new Page(
                        new Transaction("from", "to", 42))
                    .MakeTheHashValid(2);

            pageWithReward.IsValid(2, Reward).Should().Be(true);
            pageWithoutReward.IsValid(2, Reward).Should().Be(false);
        }

        [Fact]
        public void pages_with_multiple_reward_transactions_are_not_valid()
        {
            var pageWithMultipleRewardTransactions =
                new Page(
                        new Transaction("from", "to", 42),
                        new Transaction("mint", "author", Reward),
                        new Transaction("mint", "author", Reward))
                    .MakeTheHashValid(2);

            pageWithMultipleRewardTransactions.IsValid(2, Reward).Should().Be(false);
        }

        [Fact]
        public void pages_with_multiple_reward_transactions_to_different_authors_are_not_valid()
        {
            var pageWithMultipleRewardTransactions =
                new Page(
                        new Transaction("from", "to", 42),
                        new Transaction("mint", "author", Reward),
                        new Transaction("mint", "another author", Reward))
                    .MakeTheHashValid(2);

            pageWithMultipleRewardTransactions.IsValid(2, Reward).Should().Be(false);
        }
    }
}