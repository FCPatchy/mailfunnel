namespace Mailfunnel.SMTP
{
    public enum SmtpCommand
    {
        Unknown,
        EHLO,
        MAIL,
        RCPT,
        DATA,
        RSET,
        VRFY,
        NOOP,
        QUIT
    }
}