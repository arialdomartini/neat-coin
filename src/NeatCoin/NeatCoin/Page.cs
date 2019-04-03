using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NeatCoin
{
    public class Page : IEnumerable<Page>
    {
        public ImmutableList<Transaction> Transactions { get; }
        public Page Parent { get; }

        public Page(ImmutableList<Transaction> transactions)
        {
            Transactions = transactions;
        }

        public Page(ImmutableList<Transaction> transactions, Page parent)
        {
            Transactions = transactions;
            Parent = parent;
        }

        public Page Append(Page childPage) => new Page(childPage.Transactions, this);

        IEnumerator<Page> IEnumerable<Page>.GetEnumerator()
        {
            var page = this;
            do
            {
                yield return page;
                page = page.Parent;
            } while (page != null);
        }

        public IEnumerator GetEnumerator() => (this as IEnumerable<Page>).GetEnumerator();
    }
}