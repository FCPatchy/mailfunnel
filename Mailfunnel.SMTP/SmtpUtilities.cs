using System.Text.RegularExpressions;

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

        public static string ExtractAuthLogin(string text)
        {
            var m = Regex.Match(text, @"login[ ]+(.+)");
            return m.Success ? m.Groups[1].Value : string.Empty;
        }
    }
}