using System;
using NeatCoin.Cryptography;
using Newtonsoft.Json;

namespace NeatCoin
{
    internal class Block
    {
        private readonly DateTimeOffset _createdAt;
        private readonly string _content;
        private readonly ICryptography _cryptography;

        internal Block(ICryptography cryptography, DateTimeOffset createdAt, string content)
        {
            _cryptography = cryptography;

            _createdAt = createdAt;
            _content = content;
        }

        public string Hash => _cryptography.HashOf(SerializeObject());

        private string SerializeObject() =>
            JsonConvert.SerializeObject(
                new
                {
                    CreatedAt = _createdAt.AsString(),
                    Content = _content
                });
    }
}