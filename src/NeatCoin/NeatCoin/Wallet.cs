using System;
using System.Collections.Immutable;
using System.Linq;

namespace NeatCoin
{
    public class Wallet
    {
        private readonly int _difficulty;
        private readonly ImmutableList<Group> _groups;

        public Wallet(ImmutableList<Group> groups, int difficulty)
        {
            _groups = groups;
            _difficulty = difficulty;
        }

        public Group Last => _groups.Last();
        public bool IsValid => _groups.All(g => g.IsValid(_difficulty));

        public Wallet Push(Group group) =>
            new Wallet(_groups.Add(group), _difficulty);

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

        public Group MakeGroup(ImmutableList<Transaction> transactionList, int nonce = 0) =>
            new Group(transactionList, LastHash(), nonce);

        public Group Mine(Group group)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var cloneWithNonce = CloneWithNonce(group, i);
                if (cloneWithNonce.IsValid(_difficulty))
                    return cloneWithNonce;
            }
            throw new Exception();
        }

        private Group CloneWithNonce(Group group, int nonce) =>
            new Group(group.Transactions, group.Parent, nonce);
    }
}