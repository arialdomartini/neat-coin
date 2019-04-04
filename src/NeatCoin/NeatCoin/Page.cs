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
        public string Author { get; set; }

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
                    Nonce,
                    Author
                });

        public Page(string parent, ImmutableList<Transaction> transactions, string author) :
            this(parent, transactions, 0, author) {}

        public Page(string parent, ImmutableList<Transaction> transactions, int nonce, string author)
        {
            Author = author;
            Nonce = nonce;
            Parent = parent;
            Transactions = transactions;
        }

        public Page(params Transaction[] transactions) : this(ImmutableList.Create(transactions)){}
        public Page(ImmutableList<Transaction> transactions) : this("", transactions, "author"){}

        public Page LinkTo(string lastTransactionHash) => new Page(lastTransactionHash, Transactions, "author");

        public int BalanceOf(string account) => Transactions.Sum(t => t.BalanceOf(account));

        public bool IsValid(int difficulty, int reward) =>
            HasAValidHash(difficulty) &&
            ContainsARewardTransaction(Author, reward);

        private bool ContainsARewardTransaction(string author, int reward) =>
            Transactions.Count(t => t.Equals(new Transaction("mint", author, reward))) == 1 &&
            Transactions.Count(t => t.IsARewardTransaction) == 1;

        public bool HasAValidHash(int difficulty) => Hash.StartsWith(new string('0', difficulty));


        public Page MakeTheHashValid(int difficulty)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var page = CloneWithNonce(i);
                if (page.HasAValidHash(difficulty))
                    return page;
            }

            throw new Exception();
        }

        private Page CloneWithNonce(int nonce) =>
            new Page(Parent, Transactions, nonce, "author");

        public Page Reward(string author, int reward) =>
            new Page(
                Parent,
                Transactions.Add(new Transaction("mint", author, reward)),
                Nonce, "author");
    }
}