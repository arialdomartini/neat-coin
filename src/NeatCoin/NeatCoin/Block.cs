using System;
using Newtonsoft.Json;

namespace NeatCoin
{
    internal class Block
    {
        private readonly DateTimeOffset _createdAt;
        private readonly string _content;
        private readonly Hash _hash;

        internal Block(Hash hash, DateTimeOffset createdAt, string content)
        {
            _hash = hash;

            _createdAt = createdAt;
            _content = content;
        }

        public string Hash => _hash.CalculateHash(SerializeObject());

        private string SerializeObject() =>
            JsonConvert.SerializeObject(
                new
                {
                    CreatedAt = _createdAt.AsString(),
                    Content = _content
                });
    }
}