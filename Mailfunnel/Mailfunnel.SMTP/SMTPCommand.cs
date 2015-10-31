using System.ComponentModel;

namespace Mailfunnel.SMTP
{
    public enum SMTPCommand
    {
        Unknown,
        [Description("EHLO")]
        EHLO,
        [Description("MAIL FROM")]
        MAILFROM,
        [Description("RCPT TO")]
        RCPTTO
    }
}
