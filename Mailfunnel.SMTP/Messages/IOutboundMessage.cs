namespace Mailfunnel.SMTP.Messages
{
    public interface IOutboundMessage
    {
        bool Multiline { get; }
        int ReplyCode { get; }

        string GetMessage();
    }
}