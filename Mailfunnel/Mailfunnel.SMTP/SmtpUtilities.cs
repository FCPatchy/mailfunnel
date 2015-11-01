namespace Mailfunnel.SMTP
{
    public static class SmtpUtilities
    {
        public static string ExtractEmailAddress(string text)
        {
            var openBracketPosition = text.IndexOf('<');
            var closedBracketPosition = text.IndexOf('>');

            if (openBracketPosition > -1 && closedBracketPosition > -1)
            {
                openBracketPosition++;

                return text.Substring(openBracketPosition, closedBracketPosition - openBracketPosition);
            }

            return null;
        }
    }
}