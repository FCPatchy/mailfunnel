namespace Mailfunnel.SMTP.Contracts
{
    public interface IMessageProcessor
    {
        SMTPCommand ProcessMessage(byte[] message);
    }
}