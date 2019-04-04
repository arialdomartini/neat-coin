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
    }
}