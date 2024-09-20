using System.Buffers;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var arguments = Arguments.Parse(args);
        }
        catch (System.ArgumentException ae)
        {
            Console.WriteLine($"\nERROR: {ae.Message}");
            ShowHelp();
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("\nUsage: JiraEtlKpis.exe <options>\n");
        Console.WriteLine("Options:");
        Console.WriteLine("\t-JiraUrl, -j: The URL of the Jira instance.");
        Console.WriteLine("\t-JiraUserName, -u: The username for authenticating with Jira.");
        Console.WriteLine("\t-JiraToken, -t: The API token or password for authenticating with Jira.");
        Console.WriteLine("\t-SqlServerConnectionString, -s: The connection string for the SQL Server database.");
        Console.WriteLine("\t-DateTime, -d: The date AND THE TIME for performing incremental updates in the ETL process.");
        Console.WriteLine($"\t\tFormat: {DateTime.Now}");
        Console.WriteLine("\nExample:");
        Console.WriteLine($"\tJiraEtlKpis.exe -j https://my.atlassian.com -u usr -k tnk -s Server=myServerAddress;Database=myDataBase;Trusted_Connection=True -d \"{DateTime.Now}\"");
    }
}

internal class Arguments
{
    private const int EXPECTED_NUMBER_OF_ARGUMENTS = 5;

    public string? JiraUrl { get; set; }
    public string? JiraUserName { get; set; }
    public string? JiraToken { get; set; }
    public string? SqlServerConnectionString { get; set; }
    public DateTime DateForIncrementalUpdate { get; set; }

    public static Arguments Parse(string[] args)
    {
        var totalArguments = args.Length;

        CheckNumberOfArguments(totalArguments);

        var arguments = new Arguments();
        for (int i = 0; i < EXPECTED_NUMBER_OF_ARGUMENTS*2; i += 2)
        {
            string argumentName = args[i].ToLower().TrimStart('-');
            string argumentValue = args[i + 1];

            switch (argumentName)
            {
                case "jiraurl":
                case "j":
                    arguments.JiraUrl = argumentValue;
                    break;
                case "jirausername":
                case "u":
                    arguments.JiraUserName = argumentValue;
                    break;
                case "jiratoken":
                case "t":
                    arguments.JiraToken = argumentValue;
                    break;
                case "sqlserverconnectionstring":
                case "s":
                    arguments.SqlServerConnectionString = argumentValue;
                    break;
                case "datetime":
                case "d":
                    arguments.DateForIncrementalUpdate = DateTime.Parse(argumentValue);
                    break;
                default:
                    Console.WriteLine($"Invalid argument: {argumentName}");
                    break;
            }
        }

        CheckAllArgumentsAreProvided(arguments);
        return arguments;
    }

    private static void CheckNumberOfArguments(int totalArguments)
    {
        if (totalArguments < EXPECTED_NUMBER_OF_ARGUMENTS)
            throw new ArgumentException("Arguments missing");
    }

    private static bool IsJiraUrl(string argumentName) => argumentName == "jiraurl" || argumentName == "j";

    private static void CheckAllArgumentsAreProvided(Arguments arguments)
    {
        if (string.IsNullOrEmpty(arguments.JiraUrl))
        {
            throw new ArgumentException("JiraUrl is not provided.");
        }

        if (string.IsNullOrEmpty(arguments.JiraUserName))
        {
            throw new ArgumentException("JiraUserName is not provided.");
        }

        if (string.IsNullOrEmpty(arguments.JiraToken))
        {
            throw new ArgumentException("JiraToken is not provided.");
        }

        if (string.IsNullOrEmpty(arguments.SqlServerConnectionString))
        {
            throw new ArgumentException("SqlServerConnectionString is not provided.");
        }

        if (arguments.DateForIncrementalUpdate == default)
        {
            throw new ArgumentException("DateForIncrementalUpdate is not provided.");
        }
    }
}