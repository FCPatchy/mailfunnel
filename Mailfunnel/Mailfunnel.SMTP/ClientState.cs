namespace Mailfunnel.SMTP
{
    public enum ClientState
    {
        Initial,
        EHLO,
        MAIL,
        RCPT,
        DATA
    }
}