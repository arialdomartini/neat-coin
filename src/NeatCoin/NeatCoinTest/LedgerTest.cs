﻿using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class LedgerTest
    {
        [Fact]
        public void should_calculate_sender_s_balance()
        {
            var ledger = new Ledger()
                .Append(Page.GetEmpty("page1", "")
                    .Append(new Transaction("A", "B", 100))
                    .Append(new Transaction("B", "A", 20)))
                .Append(Page.GetEmpty("page2", "page1")
                    .Append(new Transaction("B", "A", 10)));

            ledger.BalanceOf("A").Should().Be(-70);
        }

        [Fact]
        public void should_calculate_receiver_s_balance()
        {
            var ledger = new Ledger()
                .Append(Page.GetEmpty("page1", "")
                    .Append(new Transaction("A", "B", 100))
                    .Append(new Transaction("B", "A", 20)))
                .Append(Page.GetEmpty("page2", "page1")
                    .Append(new Transaction("B", "A", 10)));

            ledger.BalanceOf("B").Should().Be(70);
        }

        [Fact]
        public void unknown_account_s_balance_should_be_0()
        {
            var ledger = new Ledger()
                .Append(Page.GetEmpty("page1", "")
                    .Append(new Transaction("A", "B", 100))
                    .Append(new Transaction("B", "A", 20)))
                .Append(Page.GetEmpty("page2", "page1")
                    .Append(new Transaction("B", "A", 10)));

            ledger.BalanceOf("unknown account").Should().Be(0);
        }

        [Fact]
        public void balance_is_not_calculated_for_unlinked_pages()
        {
            var ledger = new Ledger()
                .Append(Page.GetEmpty("page1", "")
                    .Append(new Transaction("A", "B", 100))
                    .Append(new Transaction("B", "A", 20)))
                .Append(Page.GetEmpty("page2", "not existing page")
                    .Append(new Transaction("B", "A", 10)));

            ledger.BalanceOf("B").Should().Be(80);
        }
    }
}