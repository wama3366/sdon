namespace Persistence.Concepts;

public class DbSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // Will be pulled from local development dotnet user-secrets

    public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};Pooling=true;";
}
