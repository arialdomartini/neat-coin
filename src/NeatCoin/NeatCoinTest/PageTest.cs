using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class PageTest
    {
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
            var page1 = new Page("some parent", ImmutableList.Create(new Transaction("from", "to", 10)));
            var page2 = new Page("another parent", ImmutableList.Create(new Transaction("from", "to", 10)));

            var hash1 = page1.Hash;
            var hash2 = page2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void pages_with_different_nonces_should_have_different_hash_values()
        {
            var page1 = new Page("some parent", ImmutableList<Transaction>.Empty, 1);
            var page2 = new Page("some parent", ImmutableList<Transaction>.Empty, 2);

            var hash1 = page1.Hash;
            var hash2 = page2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void pages_with_hashes_not_matching_difficulty_are_not_valid()
        {
            var sut = new Page(new Transaction("from", "to", 42));

            sut.IsValid(2).Should().Be(false);
            sut.Hash.Should().NotStartWith("00");
        }

        [Fact]
        public void pages_with_hashes_matching_difficulty_are_valid()
        {
            var page =
                new Page(new Transaction("from", "to", 42))
                    .Validate(2);

            page.IsValid(1).Should().Be(true);
            page.Hash.Should().StartWith("00");
        }
    }
}