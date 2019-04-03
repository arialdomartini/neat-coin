using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class PageTest
    {
        [Fact]
        public void should_be_invalid_if_hash_does_not_match_difficulty()
        {
            var transaction = new Transaction("from", "to", 100);

            var sut = new Page(transaction);

            sut.IsValid(2).Should().Be(false);
            sut.Hash.Should().NotStartWith("00");
        }

        [Fact]
        public void should_be_valid_if_hash_match_difficulty()
        {
            var transaction = new Transaction("from", "to", 100);
            var page = new Page(transaction);
            const int difficulty = 2;

            var result = page.CalculateNonce(difficulty);

            result.IsValid(2).Should().Be(true);
            result.Hash.Should().StartWith("00");
        }
    }
}