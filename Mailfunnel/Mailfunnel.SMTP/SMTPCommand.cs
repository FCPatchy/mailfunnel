namespace Mailfunnel.SMTP
{
    public enum SMTPCommand
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