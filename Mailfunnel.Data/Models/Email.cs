namespace Mailfunnel.Data.Models
{
    public class Email
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string BodyPlain { get; set; }
    }
}
