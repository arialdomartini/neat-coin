using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Ledger
    {
        public ImmutableList<Page> Pages;

        private Ledger(ImmutableList<Page> pages)
        {
            Pages = pages;
        }

        public Ledger(params Page[] pages) : this(ImmutableList.Create(pages)) { }

        public int Balance(string account) =>
            Balance(account, AsReceiver())
            - Balance(account, AsSender());

        private static Func<Transaction, string> AsSender() => t => t.Sender;
        private static Func<Transaction, string> AsReceiver() => t => t.Receiver;

        private int Balance(string account, Func<Transaction, string> role)
        {
            var root = Pages.First(p => p.Parent == null);
            return root.CalculateBalance(account, role, Pages);
            
        }
    }
}