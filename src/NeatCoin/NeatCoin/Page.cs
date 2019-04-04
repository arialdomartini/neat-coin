using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NeatCoin
{
    public struct Page
    {
        public ImmutableList<Transaction> Transactions { get; }
        public string Parent { get; }

        public string Hash =>
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        new {Transactions, Parent})));

        public Page(string parent, ImmutableList<Transaction> transactions)
        {
            Parent = parent;
            Transactions = transactions;
        }

        public Page(params Transaction[] transactions) : this("", ImmutableList.Create(transactions)){}

        public int BalanceOf(string account) => Transactions.Sum(t => t.BalanceOf(account));
    }
}