using System;
using System.Xml.XPath;
using FluentAssertions;
using NeatCoin;
using NeatCoin.Cryptography;
using Xunit;
using SHA256 = NeatCoin.Cryptography.SHA256;

namespace NeatCoinTest
{
    public class BlockTest
    {
        private readonly SHA256 _sha256;

        public BlockTest()
        {
            _sha256 = new SHA256();
        }

        [Fact]
        public void blocks_created_in_different_moments_should_have_different_hash_values()
        {
            var now = DateTimeOffset.UtcNow;
            var block1 = new Block(_sha256, now, "same content");
            var block2 = new Block(_sha256, now.AddMilliseconds(1), "same content");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void blocks_with_different_contents_should_have_different_hash_values()
        {
            var sameMoment = DateTimeOffset.UtcNow;
            var block1 = new Block(_sha256, sameMoment, "content1");
            var block2 = new Block(_sha256, sameMoment, "content2");

            var hash1 = block1.Hash;
            var hash2 = block2.Hash;

            hash1.Should().NotBe(hash2);
        }
    }

    public class BlockChainTest
    {
        private readonly BlockChain _sut;

        public BlockChainTest()
        {
            _sut = new BlockChain();
        }

        [Fact]
        public void can_host_a_block()
        {
            var block = new Block(new SHA256(), DateTimeOffset.UtcNow, "some content");

            _sut.Push(block);
            var result = _sut.GetLatest();

            result.Should().Be(block);
        }
    }
}