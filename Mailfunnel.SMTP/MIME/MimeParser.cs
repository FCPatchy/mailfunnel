using System.IO;
using System.Text;
using MimeKit;

namespace Mailfunnel.SMTP.MIME
{
    public class MimeParser : IMimeParser
    {
        public void ParseMessage(EmailMessage message)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message.Message)))
            {
                var mimeMessage = MimeMessage.Load(stream);
                message.Message = mimeMessage.HtmlBody;
                message.Subject = mimeMessage.Subject;
                message.Date = mimeMessage.Date;
            }
        }
    }
}