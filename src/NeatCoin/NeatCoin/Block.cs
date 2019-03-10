using System;

namespace NeatCoin
{
    internal class Block
    {
        private readonly DateTimeOffset _createdAt;
        private readonly Hash _hash;

        internal Block(DateTimeOffset createdAt, Hash hash)
        {
            _createdAt = createdAt;
            _hash = hash;
        }

        public string Hash => _hash.CalculateHash(_createdAt.AsString());
    }
}