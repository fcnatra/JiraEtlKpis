internal class Arguments
{
    private const int EXPECTED_NUMBER_OF_ARGUMENTS = 10;

    public string? JiraUrl { get; set; }
    public string? JiraUserName { get; set; }
    public string? JiraToken { get; set; }
    public string? SqlServerConnectionString { get; set; }
    public DateTime DateForIncrementalUpdate { get; set; }

    internal static Arguments Parse(string[] args)
    {
        var totalArguments = args.Length;
        
        CheckNumberOfArguments(totalArguments);

        Arguments arguments = Assign(args);

        CheckAllArgumentsAreProvided(arguments);

        return arguments;
    }

    private static Arguments Assign(string[] args)
    {
        var arguments = new Arguments();
        for (int i = 0; i < EXPECTED_NUMBER_OF_ARGUMENTS; i += 2)
        {
            (string argumentName, string argumentValue) = ExtractArgumentPair(args, i);

            if (Assignment.TryGetValue(argumentName, out var assign)) assign(arguments, argumentValue);
            else throw new ArgumentException($"Invalid argument: {argumentName}");
        }

        return arguments;

        // switch (argumentName)
        // {
        //     case "jiraurl":
        //     case "j":
        //         arguments.JiraUrl = argumentValue;
        //         break;
        //     case "jirausername":
        //     case "u":
        //         arguments.JiraUserName = argumentValue;
        //         break;
        //     case "jiratoken":
        //     case "t":
        //         arguments.JiraToken = argumentValue;
        //         break;
        //     case "sqlserverconnectionstring":
        //     case "s":
        //         arguments.SqlServerConnectionString = argumentValue;
        //         break;
        //     case "datetime":
        //     case "d":
        //         arguments.DateForIncrementalUpdate = DateTime.Parse(argumentValue);
        //         break;
        //     default:
        //         Console.WriteLine($"Invalid argument: {argumentName}");
        //         break;
        // }
    }

    private static (string argumentName, string argumentValue) ExtractArgumentPair(string[] args, int i)
    {
        string argumentName = args[i].ToLower().TrimStart('-');
        string argumentValue = args[i + 1];

        return (argumentName, argumentValue);
    }

    private static void CheckNumberOfArguments(int totalArguments)
    {
        if (totalArguments < EXPECTED_NUMBER_OF_ARGUMENTS)
            throw new ArgumentException("Arguments missing");
    }

    private static void CheckAllArgumentsAreProvided(Arguments arguments)
    {
        if (string.IsNullOrEmpty(arguments.JiraUrl))
            throw new ArgumentException("JiraUrl is not provided.");

        if (string.IsNullOrEmpty(arguments.JiraUserName))
            throw new ArgumentException("JiraUserName is not provided.");

        if (string.IsNullOrEmpty(arguments.JiraToken))
            throw new ArgumentException("JiraToken is not provided.");

        if (string.IsNullOrEmpty(arguments.SqlServerConnectionString))
            throw new ArgumentException("SqlServerConnectionString is not provided.");

        if (arguments.DateForIncrementalUpdate == default)
            throw new ArgumentException("DateForIncrementalUpdate is not provided.");
    }

    private static Dictionary<string, Action<Arguments, string>> Assignment = new ()
    {
        { "jiraurl", (a, value) => a.JiraUrl = value },
        { "j", (a, value) => a.JiraUrl = value },

        { "jirausername", (a, value) => a.JiraUserName = value },
        { "u", (a, value) => a.JiraUserName = value },

        { "jiratoken", (a, value) => a.JiraToken = value },
        { "t", (a, value) => a.JiraToken = value },

        { "sqlserverconnectionstring", (a, value) => a.SqlServerConnectionString = value },
        { "s", (a, value) => a.SqlServerConnectionString = value },

        { "datetime", (a, value) => a.DateForIncrementalUpdate = DateTime.Parse(value) },
        { "d", (a, value) => a.DateForIncrementalUpdate = DateTime.Parse(value) },
    };

    internal static void ShowHelp()
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