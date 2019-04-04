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
        private ImmutableList<Transaction> Transactions { get; }

        public string Hash =>
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        new {Number, Transactions})));

        public Page(int number, params Transaction[] transactions)
        {
            Number = number;
            Transactions = ImmutableList.Create(transactions);
        }

        public int BalanceOf(string account) => Transactions.Sum(t => t.BalanceOf(account));
    }
}