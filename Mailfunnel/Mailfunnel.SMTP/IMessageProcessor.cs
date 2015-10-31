namespace Mailfunnel.SMTP
{
    public interface IMessageProcessor
    {
        SMTPCommand ProcessMessage(byte[] message);
    }
}