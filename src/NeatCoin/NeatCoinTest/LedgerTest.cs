using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void can_host_one_transaction()
        {
            var sut = new Ledger(new Transaction("sender", "receiver", 10));

            var result = sut.Transaction;
            
            result.Should().Be(new Transaction("sender", "receiver", 10));
        }
    }
}