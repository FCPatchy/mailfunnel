namespace Mailfunnel.SMTP
{
    public enum SmtpCommand
    {
        Unknown,
        EHLO,
        AUTH,
        MAIL,
        RCPT,
        DATA,
        RSET,
        VRFY,
        NOOP,
        QUIT
    }
}