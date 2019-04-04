using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace NeatCoin
{
    public struct Page
    {
        public ImmutableList<Transaction> Transactions { get; }
        public string Parent { get; }
        public int Nonce { get; }

        public string Hash => CalculateHash();

        private string CalculateHash()
        {
            using (var sha1 = new SHA1Managed())
            {
                return Convert.ToBase64String(
                    sha1.ComputeHash(
                        Encoding.UTF8.GetBytes(Serialized)));
            }
        }

        private string Serialized =>
            JsonConvert.SerializeObject(
                new
                {
                    Transactions,
                    Parent,
                    Nonce
                });

        public Page(string parent, ImmutableList<Transaction> transactions) : this(parent, transactions, 0) {}

        public Page(string parent, ImmutableList<Transaction> transactions, int nonce)
        {
            Nonce = nonce;
            Parent = parent;
            Transactions = transactions;
        }

        public Page(params Transaction[] transactions) : this("", ImmutableList.Create(transactions)){}

        public Page LinkTo(string lastTransactionHash) => new Page(lastTransactionHash, Transactions);

        public int BalanceOf(string account) => Transactions.Sum(t => t.BalanceOf(account));

        public bool IsValid(int difficulty) => Hash.StartsWith(new string('0', difficulty));


        public Page Validate(int difficulty)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var page = CloneWithNonce(i);
                if (page.IsValid(difficulty))
                    return page;
            }

            throw new Exception();
        }

        private Page CloneWithNonce(int nonce) =>
            new Page(Parent, Transactions, nonce);
    }
}