using System.Configuration;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace Mailfunnel.Data.Infrastructure
{
    public class DatabaseInitialiser : IDatabaseInitialiser
    {
        public void InitialiseDatabase()
        {
            var dbPath = ConfigurationManager.AppSettings["ConnectionString"];
            if (!File.Exists(dbPath))
                SQLiteConnection.CreateFile(dbPath);
        }

        public void SeedDatabase()
        {
            using (var connection = new DatabaseConnection())
            {
                connection.SQLiteConnection.Open();

                connection.SQLiteConnection.Execute(@"
                    CREATE TABLE IF NOT EXISTS [Emails] (
                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [From] NVARCHAR(100) NOT NULL,
                        [To] NVARCHAR(100) NOT NULL,
                        [Subject] NVARCHAR(200) NOT NULL,
                        [BodyText] NVARCHAR(MAX) NULL,
                        [BodyHtml] NVARCHAR(MAX) NULL
                    )");
            }
        }
    }
}
