
namespace Mailfunnel.SMTP.Messages
{
    public class ClientMessage
    {
        public MessageType MessageType { get; set; }
        public SmtpCommand SMTPCommand { get; set; }
        public string MessageText { get; set; }
    }
}