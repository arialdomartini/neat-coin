using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NeatCoin
{
    public struct Page
    {
        public int Number { get; }
        public ImmutableList<Transaction> Transactions { get; }

        public string Hash =>
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        new {Number, Transactions})));

        public Page(params Transaction[] transactions) : this(0, transactions) {}

        public Page(int number, ImmutableList<Transaction> transactions)
        {
            Number = number;
            Transactions = transactions;
        }

        public Page(int number, params Transaction[] transactions) : this(number, ImmutableList.Create(transactions)){}

        public int BalanceOf(string account) => Transactions.Sum(t => t.BalanceOf(account));
    }
}