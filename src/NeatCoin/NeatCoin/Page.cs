using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace NeatCoin
{
    public class Page
    {
        public ImmutableList<Transaction> Transactions { get; }
        public string Parent { get; }

        private Page(ImmutableList<Transaction> transactions)
        {
            Transactions = transactions;
        }

        public Page(params Transaction[] transactions) : this(ImmutableList.Create(transactions)) {}

        public Page(ImmutableList<Transaction> transactions, string parent)
        {
            Transactions = transactions;
            Parent = parent;
        }

        public Page LinkTo(Page parent) => new Page(Transactions, parent.Hash);

        public string Hash => CalculateHash(
            Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(
                    new
                    {
                        Transactions,
                        Parent
                    })));

        public int CalculateBalance(string account, Func<Transaction, string> role, ImmutableList<Page> pages)
        {
            var balance = Balance(account, role);
            var children = pages.Where(p => p.Parent == Hash).ToList();
            var sum = children
                .Sum(p => p.CalculateBalance(account, role, pages));
            return balance + sum;
        }

        private int Balance(string account, Func<Transaction, string> role)
        {
            return Transactions.Where(t => role(t) == account)
                .Sum(t => t.Amount);
        }

        public string CalculateHash(byte [] bytes)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}