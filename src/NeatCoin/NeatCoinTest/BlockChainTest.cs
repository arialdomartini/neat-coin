using System.Collections.Immutable;
using FluentAssertions;
using NeatCoin;
using Xunit;
using Block = NeatCoin.Block;

namespace NeatCoinTest
{
    public class BlockChainTest
    {
        private readonly BlockChain _sut;
        private const int Difficulty = 2;
        private const int RewardAmount = 50;
        private readonly ImmutableList<Block> _emptyList = ImmutableList.Create<Block>();

        private static readonly ImmutableList<Transaction> TransactionList1 = ImmutableList.Create(
            new Transaction("from", "to", 100),
            new Transaction("from", "to", 100));

        private static readonly ImmutableList<Transaction> TransactionList2 = ImmutableList.Create(
            new Transaction("to", "from", 50));

        public BlockChainTest()
        {
            _sut = new BlockChain(_emptyList, Difficulty, RewardAmount);
        }

        [Fact]
        public void should_retrieve_accounts_balances_with_multiple_transactions()
        {
            var wallet = _sut
                .Push(_sut.MakeBlock(TransactionList1))
                .Push(_sut.MakeBlock(TransactionList2));

            wallet.BalanceOf("from").Should().Be(-150);
            wallet.BalanceOf("to").Should().Be(150);
        }

        [Fact]
        public void should_retrieve_last_block()
        {
            var block1 = _sut.MakeBlock(TransactionList1);
            var wallet = _sut
                .Push(block1);

            wallet.Last.Should().Be(block1);

            var block2 = wallet.MakeBlock(TransactionList2);
            wallet = wallet
                .Push(block2);

            wallet.Last.Should().Be(block2);
        }

        [Fact]
        public void should_retrieve_blocks_by_hash()
        {
            var block = _sut.MakeBlock(TransactionList1);
            var wallet = _sut
                .Push(block);

            var result = wallet.GetBlock(block.Hash);

            result.Should().Be(block);
        }

        [Fact]
        public void genesis_block_should_have_no_parents()
        {
            var block = _sut.MakeBlock(TransactionList1);

            var parent = block.Parent;

            parent.Should().Be(default(Hash));
        }

        [Fact]
        public void block_should_be_chained()
        {
            var block1 = _sut.MakeBlock(TransactionList1);
            var wallet = _sut.Push(block1);

            var block2 = wallet.MakeBlock(TransactionList2);
            wallet = wallet.Push(block2);

            var last = wallet.Last;
            var parent = wallet.GetBlock(last.Parent);

            parent.Should().Be(block1);
        }

        [Fact]
        public void should_not_be_valid_if_blocks_are_not_mined()
        {
            var wallet = _sut.Push(_sut.MakeBlock(TransactionList1));

            var result = wallet.IsValid;

            result.Should().Be(false);
        }

        [Fact]
        public void should_be_valid_if_blocks_are_mined()
        {
            var block = _sut.MakeBlock(TransactionList1);
            var mined = _sut.Mine(block, "some miner");
            var wallet = _sut.Push(mined);

            var result = wallet.IsValid;

            result.Should().Be(true);
        }

        [Fact]
        public void blocks_are_not_valid_if_their_hash_do_not_match_difficulty()
        {
            var block = _sut.MakeBlock(TransactionList1);

            var result = block.IsValid(Difficulty);

            result.Should().Be(false);
        }

        [Fact]
        public void blocks_are_valid_if_their_hash_matches_difficulty()
        {
            var block = _sut.MakeBlock(TransactionList1);
            var mined = _sut.Mine(block, "some miner");

            var result = mined.IsValid(Difficulty);

            result.Should().Be(true);
        }

        [Fact]
        public void unmined_blocks_should_contain_no_reward_transactions()
        {
            var block = _sut.MakeBlock(TransactionList1);

            var rewardTransaction = block.RewardTransaction;

            rewardTransaction.Should().Be(null);
        }

        [Fact]
        public void mined_blocks_should_contain_a_reward_transaction()
        {
            var block = _sut.MakeBlock(TransactionList1);
            var mined = _sut.Mine(block, "some miner");

            var rewardTransaction = mined.RewardTransaction;

            rewardTransaction.Should().BeEquivalentTo(
                new Transaction(
                    "mint",
                    "some miner",
                    RewardAmount));
        }

        [Fact]
        public void miner_should_receive_a_reward_amount()
        {
            _sut.BalanceOf("some lucky miner").Should().Be(0);

            var block = _sut.MakeBlock(ImmutableList<Transaction>.Empty);
            var mined = _sut.Mine(block, "some lucky miner");
            var blockChain = _sut.Push(mined);

            blockChain.BalanceOf("some lucky miner").Should().Be(RewardAmount);
        }
    }
}