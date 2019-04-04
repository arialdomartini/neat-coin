using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class PageTest
    {
        [Fact]
        public void should_iterate_pages()
        {
            var page1 = new Page(1);
            var page2 = new Page(2);
            var page3 = new Page(3);
            var pages = ImmutableList.Create(page1, page3, page2);

            var n = 1;
            foreach (var page in pages.IterateFrom(page1))
            {
                page.Number.Should().Be(n++);
            }
        }
    }
}