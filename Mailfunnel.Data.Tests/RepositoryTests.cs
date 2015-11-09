using System.Data.Unqlite;
using Mailfunnel.Data.Entities;
using Mailfunnel.Data.Repository;
using NUnit.Framework;

namespace Mailfunnel.Data.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void EmailRepositoryTest()
        {
            // Arrange
            var repo = new EmailRepository();
            repo.Open(":mem:", Unqlite_Open.READWRITE);

            // Act
            var x = repo.Add(new EmailEntity
            {
                Name = "Test"
            });

            var y = repo.Add(new EmailEntity
            {
                Name = "Test 2"
            });

            // Assert
            Assert.IsNotNull(x);
            Assert.IsNotNull(y);
            Assert.AreEqual(0, x.__id);
            Assert.AreEqual(1, y.__id);
        }
    }
}