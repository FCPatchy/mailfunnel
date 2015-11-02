namespace Mailfunnel.SMTP.Messages
{
    public interface IMessageProcessor
    {
        ClientMessage ProcessMessage(string message);
    }
}