namespace Mailfunnel.SMTP.Messages
{
    public interface IMessageProcessor
    {
        ClientMessage ProcessMessage(string message);
        string GenerateMessage(IOutboundMessage message);
    }
}