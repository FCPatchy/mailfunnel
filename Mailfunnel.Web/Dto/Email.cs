namespace Mailfunnel.Web.Dto
{
    public class Email
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
    }
}
