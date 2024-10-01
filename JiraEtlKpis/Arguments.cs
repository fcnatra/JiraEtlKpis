using System.Data.SqlClient;

internal class Arguments
{
    private const int EXPECTED_NUMBER_OF_ARGUMENTS = 5;

    public string? JiraUrl { get; set; }
    public string? JiraUserName { get; set; }
    public string? JiraToken { get; set; }
    public string? SqlServerConnectionString { get; set; }
    public DateTime DateForIncrementalUpdate { get; set; }

    internal static Arguments Parse(string[] args)
    {
        CheckNumberOfArguments(args.Length);

        Arguments arguments = new();
        arguments.Assign(args);

        CheckAllArgumentsAreProvided(arguments);

        return arguments;
    }

    private Arguments Assign(string[] args)
    {
        JiraUrl = args[0];
        JiraUserName = args[1];
        JiraToken = args[2];
        SqlServerConnectionString = args[3];
        if (DateTime.TryParse(args[4], out var datetimeArg))
            DateForIncrementalUpdate = datetimeArg;

        return this;
    }

    private static void CheckNumberOfArguments(int totalArguments)
    {
        if (totalArguments < EXPECTED_NUMBER_OF_ARGUMENTS)
            throw new ArgumentException("Arguments missing");
    }

    private static void CheckAllArgumentsAreProvided(Arguments arguments)
    {
        if (!IsValidUrl(arguments))
            throw new ArgumentException("Jira Url is not valid");

        if (string.IsNullOrEmpty(arguments.JiraUserName))
            throw new ArgumentException("Jira UserName is not valid.");

        if (string.IsNullOrEmpty(arguments.JiraToken))
            throw new ArgumentException("Jira Token is not valid.");

        if (!IsValidSqlConnectionString(arguments))
            throw new ArgumentException("SqlServer ConnectionString is not valid.");

        if (!IsValidDateTime(arguments))
            throw new ArgumentException("Date For Incremental Update is not valid.");
    }

    private static bool IsValidDateTime(Arguments arguments) => arguments.DateForIncrementalUpdate != default;

    private static bool IsValidSqlConnectionString(Arguments arguments)
    {
        try
        {
            new SqlConnectionStringBuilder(arguments.SqlServerConnectionString);
            return true;
        }
        catch (ArgumentException)
        { return false; }
    }

    private static bool IsValidUrl(Arguments arguments) =>  !string.IsNullOrEmpty(arguments.JiraUrl) &&  Uri.TryCreate(arguments.JiraUrl, UriKind.Absolute, out _);

    internal static void ShowHelp()
    {
        Console.WriteLine("\nUsage: JiraEtlKpis.exe <Jira Url> <Jira UserName> <Jira Token> <SqlServer Connection String> <DateTime>\n");
        Console.WriteLine("Options:");
        Console.WriteLine("\tJira Url: The URL of the Jira instance.");
        Console.WriteLine("\tJira UserName: The username for authenticating with Jira.");
        Console.WriteLine("\tJira Token: The API token or password for authenticating with Jira.");
        Console.WriteLine("\tSqlServer ConnectionString: The connection string for the SQL Server database.");
        Console.WriteLine("\tDateTime: The date AND THE TIME for performing incremental updates in the ETL process.");
        Console.WriteLine($"\t\tFormat: {DateTime.Now}");
        Console.WriteLine("\nExample:");
        Console.WriteLine($"\tJiraEtlKpis.exe https://my.atlassian.com usr tkn \"Server=myServerAddress;Database=myDataBase;Trusted_Connection=True\" \"{DateTime.Now}\"");
    }
}