using System;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Wallet
    {
        private readonly ImmutableList<Group> _groups;

        public Wallet()
        {
            _groups = ImmutableList.Create<Group>();
        }

        private Wallet(ImmutableList<Group> groups)
        {
            _groups = groups;
        }

        public Group Last => _groups.Last();

        public Wallet Push(Group group) =>
            new Wallet(_groups.Add(group));

        private Hash LastHash()
        {
            var lastHash = default(Hash);
            if(_groups.Any())
                lastHash = Last.Hash;
            return lastHash;
        }

        public int BalanceOf(Account account) =>
            Total(Transaction.IsReceiver(account)) - Total(Transaction.IsSender(account));

        private int Total(Func<Transaction, bool> condition) =>
            _groups
                .SelectMany(g => g.Transactions)
                .Where(condition)
                .Sum(t => t.Amount);

        public Group GetGroup(string hash) =>
            _groups.Find(g => g.Hash == hash);

        public Group MakeGroup(ImmutableList<Transaction> transactionList) =>
            new Group(transactionList, LastHash());
    }
}