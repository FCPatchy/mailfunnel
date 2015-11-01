using NUnit.Framework;

namespace Mailfunnel.SMTP.Tests
{
    [TestFixture]
    public class SmtpUtilitiesTests
    {
        [Test]
        public void ExtractEmailAddress_Extracts_from_MAIL_command()
        {
            var cmdText = "MAIL from: <sender@example.com>";
            var extractedAddress = SmtpUtilities.ExtractEmailAddress(cmdText);

            Assert.AreEqual("sender@example.com", extractedAddress);
        }

        [Test]
        public void ExtractEmailAddress_Extracts_from_RCPT_command()
        {
            var cmdText = "RCPT TO: <recipient@example.com>";
            var extractedAddress = SmtpUtilities.ExtractEmailAddress(cmdText);

            Assert.AreEqual("recipient@example.com", extractedAddress);
        }

        [Test]
        public void ExtractEmailAddress_Returns_null_for_invalid_address()
        {
            var cmdText = "RCPT TO: <something@somewhere.com"; // Missing >
            var extractedAddress = SmtpUtilities.ExtractEmailAddress(cmdText);

            Assert.Null(extractedAddress);
        }
    }
}
