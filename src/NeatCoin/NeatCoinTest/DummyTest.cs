using FluentAssertions;
using Xunit;

namespace NeatCoinTest
{
    public class DummyTest
    {
        [Fact]
        public void should_pass()
        {
            "friends".Should().Be("friends");
        }
    }
}