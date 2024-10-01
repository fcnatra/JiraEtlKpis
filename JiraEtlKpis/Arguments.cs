using System.Data.SqlClient;
using Interfaces;

public class Arguments : IJiraArguments
{
    private const int EXPECTED_NUMBER_OF_ARGUMENTS = 4;

    public string? Url { get; set; }
    public string? Token { get; set; }
    public string? SqlServerConnectionString { get; set; }
    public DateTime DateForIncrementalUpdate { get; set; }

    internal static Arguments Parse(string[] args)
    {
        CheckNumberOfArguments(args.Length);

        Arguments arguments = new();
        arguments.Assign(args);

        arguments.Check();

        return arguments;
    }

    private Arguments Assign(string[] args)
    {
        Url = args[0];
        Token = args[1];
        SqlServerConnectionString = args[2];
        if (DateTime.TryParse(args[3], out var datetimeArg))
            DateForIncrementalUpdate = datetimeArg;

        return this;
    }

    private static void CheckNumberOfArguments(int totalArguments)
    {
        if (totalArguments < EXPECTED_NUMBER_OF_ARGUMENTS)
            throw new ArgumentException("Arguments missing");
    }

    public void Check()
    {
        if (!IsValidUrl())
            throw new ArgumentException("Jira Url is not valid");

        if (string.IsNullOrEmpty(Token))
            throw new ArgumentException("Jira Token is not valid.");

        if (!IsValidSqlConnectionString())
            throw new ArgumentException("SqlServer ConnectionString is not valid.");

        if (!IsValidDateTime())
            throw new ArgumentException("Date For Incremental Update is not valid.");
    }

    private bool IsValidDateTime() => DateForIncrementalUpdate != default;

    private bool IsValidSqlConnectionString()
    {
        try
        {
            new SqlConnectionStringBuilder(SqlServerConnectionString);
            return true;
        }
        catch (ArgumentException)
        { return false; }
    }

    private bool IsValidUrl() =>  !string.IsNullOrEmpty(Url) &&  Uri.TryCreate(Url, UriKind.Absolute, out _);

    internal static void ShowHelp()
    {
        Console.WriteLine("\nUsage: JiraEtlKpis.exe <Jira Url> <Jira Token> <SqlServer Connection String> <DateTime>\n");
        Console.WriteLine("Options:");
        Console.WriteLine("\tJira Url: The URL of the Jira instance.");
        Console.WriteLine("\tJira Token: The API token or password for authenticating with Jira.");
        Console.WriteLine("\tSqlServer ConnectionString: The connection string for the SQL Server database.");
        Console.WriteLine("\tDateTime: The date AND THE TIME for performing incremental updates in the ETL process.");
        Console.WriteLine($"\t\tFormat: {DateTime.Now}");
        Console.WriteLine("\nExample:");
        Console.WriteLine($"\tJiraEtlKpis.exe https://my.atlassian.com tkn \"Server=myServerAddress;Database=myDataBase;Trusted_Connection=True\" \"{DateTime.Now}\"");
    }
}
