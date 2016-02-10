using System.Collections.Generic;
using System.Linq;
using Dapper;
using Mailfunnel.Data.Infrastructure;
using Mailfunnel.Data.Models;

namespace Mailfunnel.Data.Repository
{
    public class EmailRepository : IEmailRepository
    {
        public IEnumerable<Email> GetAllEmails()
        {
            using (var conn = new DatabaseConnection())
            {
                return conn.SQLiteConnection.Query<Email>("SELECT * FROM Emails");
            }
        }

        public Email GetEmail(int id)
        {
            using (var conn = new DatabaseConnection())
            {
                return conn.SQLiteConnection.Query<Email>($"SELECT * FROM Emails WHERE Id = {id}").FirstOrDefault();
            }
        }

        public void SetEmail(Email email)
        {
            using (var conn = new DatabaseConnection())
            {
                if (email.Id > 0)
                {
                    // Todo?
                }
                else
                {
                    conn.SQLiteConnection.Execute(@"INSERT INTO Emails ([Id], [GroupId], [From], [To], [Subject], [BodyHtml]) VALUES (@Id, @GroupId, @From, @To, @Subject, @BodyHtml)", email);
                }
            }
        }
    }
}
