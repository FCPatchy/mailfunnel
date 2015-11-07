namespace Mailfunnel.SMTP.Messages
{
    public interface IOutboundMessage
    {
        int ReplyCode { get; }

        string GetMessage();
    }
}