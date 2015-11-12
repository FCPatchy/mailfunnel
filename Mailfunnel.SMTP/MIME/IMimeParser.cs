namespace Mailfunnel.SMTP.MIME
{
    public interface IMimeParser
    {
        void ParseMessage(EmailMessage message);
    }
}