using System;
using System.Configuration;
using System.Data.SQLite;

namespace Mailfunnel.Data.Infrastructure
{
    public sealed class DatabaseConnection : IDisposable
    {
        public SQLiteConnection SQLiteConnection { get; }

        public DatabaseConnection()
        {
            SQLiteConnection = new SQLiteConnection(GetConnectionString());
        }

        public void Dispose()
        {
            SQLiteConnection.Dispose();
        }

        private static string GetConnectionString()
        {
            return $"Data Source={ConfigurationManager.AppSettings["ConnectionString"]};Version=3;";
        }
    }
}
