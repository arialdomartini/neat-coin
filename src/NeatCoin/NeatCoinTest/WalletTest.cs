using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;
using Group = NeatCoin.Group;

namespace NeatCoinTest
{
    public class WalletTest
    {
        private readonly Wallet _sut;
        private const int Difficulty = 2;
        private readonly ImmutableList<Group> _emptyList = ImmutableList.Create<Group>();

        private static readonly ImmutableList<Transaction> TransactionList1 = ImmutableList.Create(
            new Transaction("from", "to", 100),
            new Transaction("from", "to", 100));

        private static readonly ImmutableList<Transaction> TransactionList2 = ImmutableList.Create(
            new Transaction("to", "from", 50));

        public WalletTest()
        {
            _sut = new Wallet(_emptyList, Difficulty);
        }

        [Fact]
        public void should_retrieve_accounts_balances_with_multiple_transactions()
        {
            var wallet = _sut
                .Push(_sut.MakeGroup(TransactionList1))
                .Push(_sut.MakeGroup(TransactionList2));

            wallet.BalanceOf("from").Should().Be(-150);
            wallet.BalanceOf("to").Should().Be(150);
        }

        [Fact]
        public void should_retrieve_last_group()
        {
            var group1 = _sut.MakeGroup(TransactionList1);
            var wallet = _sut
                .Push(group1);

            wallet.Last.Should().Be(group1);

            var group2 = wallet.MakeGroup(TransactionList2);
            wallet = wallet
                .Push(group2);

            wallet.Last.Should().Be(group2);
        }

        [Fact]
        public void should_retrieve_groups_by_hash()
        {
            var group = _sut.MakeGroup(TransactionList1);
            var wallet = _sut
                .Push(group);

            var result = wallet.GetGroup(group.Hash);

            result.Should().Be(group);
        }

        [Fact]
        public void genesis_group_should_have_no_parents()
        {
            var group = _sut.MakeGroup(TransactionList1);

            var parent = group.Parent;

            parent.Should().Be(default(Hash));
        }

        [Fact]
        public void group_should_be_chained()
        {
            var group1 = _sut.MakeGroup(TransactionList1);
            var wallet = _sut.Push(group1);

            var group2 = wallet.MakeGroup(TransactionList2);
            wallet = wallet.Push(group2);

            var last = wallet.Last;
            var parent = wallet.GetGroup(last.Parent);

            parent.Should().Be(group1);
        }

        [Fact]
        public void should_not_be_valid_if_groups_are_not_mined()
        {
            var wallet = _sut.Push(_sut.MakeGroup(TransactionList1));

            var result = wallet.IsValid;

            result.Should().Be(false);
        }

        [Fact]
        public void should_be_valid_if_groups_are_mined()
        {
            var group = _sut.MakeGroup(TransactionList1);
            var mined = _sut.Mine(group);
            var wallet = _sut.Push(mined);

            var result = wallet.IsValid;

            result.Should().Be(true);
        }

        [Fact]
        public void groups_are_not_valid_if_their_hash_do_not_match_difficulty()
        {
            var group = _sut.MakeGroup(TransactionList1);

            var result = group.IsValid(Difficulty);

            result.Should().Be(false);
        }

        [Fact]
        public void groups_are_valid_if_their_hash_matches_difficulty()
        {
            var group = _sut.MakeGroup(TransactionList1);
            var mined = _sut.Mine(group);

            var result = mined.IsValid(Difficulty);

            result.Should().Be(true);
        }
    }
}