using System;
using System.Collections.Generic;
using NeatCoin.Cryptography;

namespace NeatCoin
{
    internal class GenesisBlock : Block
    {
        private static readonly DateTimeOffset CreationDate = new DateTimeOffset(2019, 3, 13, 7, 0, 2, TimeSpan.FromHours(1));
        private const string NoParent = "0";
        private static readonly List<Transaction> GenesisContent = new List<Transaction>();
        private const int GenesisNonce = 0;

        public GenesisBlock(ICryptography cryptography, int difficulty) :
            base(
                cryptography,
                CreationDate,
                GenesisContent,
                NoParent,
                difficulty,
                GenesisNonce
            )
        {

        }
    }
}