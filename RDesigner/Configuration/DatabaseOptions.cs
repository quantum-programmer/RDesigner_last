using Npgsql;

namespace RDesigner.Configuration;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string DBName { get; init; } = string.Empty;

    public int Port { get; init; }

    public string Host { get; init; } = "localhost";

    public string CreateConnectionString()
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = Host,
            Database = DBName,
            Port = Port,
            Username = Username,
            Password = Password
        };

        return builder.ConnectionString;
    }
}
