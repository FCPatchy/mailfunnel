using System.Collections.Generic;
using System.Linq;
using Mailfunnel.Data.Repository;
using Mailfunnel.Web.Dto;

namespace Mailfunnel.Web.Managers
{
    public class EmailManager : IEmailManager
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IGroupRepository _groupRepository;

        public EmailManager(IEmailRepository emailRepository, IGroupRepository groupRepository)
        {
            _emailRepository = emailRepository;
            _groupRepository = groupRepository;
        }

        public IEnumerable<Email> GetAllEmails()
        {
            var emails = _emailRepository.GetAllEmails();
            if (!emails.Any())
                return null;

            return (from email in emails
                    select new Email
                    {
                        Subject = email.Subject
                    });
        }

        public Email GetEmail(int id)
        {
            var email = _emailRepository.GetEmail(id);
            if (email == null)
                return null;

            return new Email
            {
                Subject = email.Subject
            };
        }
    }
}
