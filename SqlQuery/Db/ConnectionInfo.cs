using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySqlConnector;
using Npgsql;

namespace SqlQuery.Db
{
    public enum ServerType
    {
        SqlServer = 0,
        Sqlite = 1,
        PostgreSql = 2,
        MySql = 3
    }

    public class ConnectionInfo
    {
        public string Name { get; set; }
        public ServerType ServerType { get; set; }
        public string Server { get; set; }
        public bool WindowsAuth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string FilePath { get; set; }
        public int Port { get; set; }
        public bool Encrypt { get; set; }

        public ConnectionInfo()
        {
            Name = string.Empty;
            Server = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Database = string.Empty;
            FilePath = string.Empty;
            Port = 5432;
            WindowsAuth = true;
        }

        public string BuildConnectionString()
        {
            switch (ServerType)
            {
                case ServerType.SqlServer:
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                        builder.DataSource = Server;
                        builder.InitialCatalog = string.IsNullOrEmpty(Database) ? "master" : Database;
                        builder.IntegratedSecurity = WindowsAuth;
                        if (!WindowsAuth)
                        {
                            builder.UserID = UserName;
                            builder.Password = Password;
                        }
                        builder.Encrypt = Encrypt;
                        return builder.ConnectionString;
                    }
                case ServerType.Sqlite:
                    {
                        SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
                        builder.DataSource = FilePath;
                        builder.Version = 3;
                        return builder.ConnectionString;
                    }
                case ServerType.PostgreSql:
                    {
                        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
                        builder.Host = Server;
                        builder.Port = Port;
                        builder.Username = UserName;
                        builder.Password = Password;
                        builder.Database = string.IsNullOrEmpty(Database) ? "postgres" : Database;
                        return builder.ConnectionString;
                    }
                case ServerType.MySql:
                    {
                        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                        builder.Server = Server;
                        builder.Port = (uint)(Port <= 0 ? 3306 : Port);
                        builder.UserID = UserName;
                        builder.Password = Password;
                        builder.Database = string.IsNullOrEmpty(Database) ? null : Database;
                        return builder.ConnectionString;
                    }
                default:
                    throw new NotSupportedException("Unsupported server type.");
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
