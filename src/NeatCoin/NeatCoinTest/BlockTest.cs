using System;
using System.Security.Cryptography;
using FluentAssertions;
using NeatCoin;
using Xunit;

namespace NeatCoinTest
{
    public class BlockTest
    {
        private readonly Hash _hash;

        public BlockTest()
        {
            _hash = new Hash(new SHA256Managed());
        }

        [Fact]
        public void blocks_created_in_different_moments_should_have_different_hash_values()
        {
            var now = DateTimeOffset.UtcNow;
            var block1 = new Block(_hash, now, "same content");
            var block2 = new Block(_hash, now.AddMilliseconds(1), "same content");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void blocks_with_different_contents_should_have_different_hash_values()
        {
            var sameMoment = DateTimeOffset.UtcNow;
            var block1 = new Block(_hash, sameMoment, "content1");
            var block2 = new Block(_hash, sameMoment, "content2");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }
    }
}