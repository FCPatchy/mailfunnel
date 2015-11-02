namespace Mailfunnel.SMTP.Messages
{
    public class ClientMessage
    {
        public SMTPCommand SMTPCommand { get; set; }
        public string MessageText { get; set; }
    }
}