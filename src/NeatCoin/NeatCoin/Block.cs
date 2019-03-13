using System;
using NeatCoin.Cryptography;
using Newtonsoft.Json;

namespace NeatCoin
{
    internal class Block
    {
        private readonly DateTimeOffset _createdAt;
        private readonly string _content;
        public string Parent { get; protected set; }
        private readonly ICryptography _cryptography;

        internal Block(ICryptography cryptography, DateTimeOffset createdAt, string content, string parent)
        {
            _cryptography = cryptography;

            _createdAt = createdAt;
            _content = content;
            Parent = parent;
        }

        public string Hash => _cryptography.HashOf(Serialized);

        private string Serialized =>
            JsonConvert.SerializeObject(
                new
                {
                    CreatedAt = _createdAt.AsString(),
                    Content = _content,
                    Parent
                });

        public bool IsChainedTo(Block last) => Parent == last.Hash;
    }
}