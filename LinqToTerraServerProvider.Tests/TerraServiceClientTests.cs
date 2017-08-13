using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace LinqToTerraServerProvider.Tests
{
    [TestFixture]
    public class TerraServiceClientTests
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Test]
        public void GetPlaceFacts_ParseJson_ShouldReturnArray()
        {
            var client = _fixture.Create<TerraServiceClient>();
            var placeFacts = client.GetPlaceFacts("Dubuque");
            
            placeFacts.First().City.Should().Be("Dubuque", "because we passed a location that is in the file.");
        }
    }
}
