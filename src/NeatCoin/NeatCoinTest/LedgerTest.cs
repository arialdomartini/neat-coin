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
            var ledger = new Ledger();

            var result = ledger.Append(new Transaction("sender", "receiver", 100));

            result.Transaction.Should().Be(new Transaction("sender", "receiver", 100));
        }
    }
}