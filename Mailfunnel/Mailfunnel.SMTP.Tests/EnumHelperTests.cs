using Mailfunnel.SMTP.Utilities;
using NUnit.Framework;

namespace Mailfunnel.SMTP.Tests
{
    [TestFixture]
    public class EnumHelperTests
    {
        [Test]
        public void TryGetEnumByDescriptionAttribute_ParsesCorrectly()
        {
            SMTPCommand command;
            EnumHelper.TryGetEnumByDescriptionAttribute("EHLO", out command);

            Assert.That(command, Is.EqualTo(SMTPCommand.EHLO));
        }
    }
}