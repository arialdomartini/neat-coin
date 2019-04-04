using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void should_contain_a_transaction()
        {
            var sut = new Ledger();

            var transaction = new Transaction("from", "to", 10);

            sut = sut.Append(transaction);

            sut.Transaction.Should().Be(transaction);
        }
    }
}