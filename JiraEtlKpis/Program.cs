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
            Console.WriteLine($"\nERROR: {ae.Message}\n");
            ShowHelp();
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("\nUsage: JiraEtlKpis.exe <options>\n");
        Console.WriteLine("Arguments:");
        Console.WriteLine("\t-JiraUrl, -j: The URL of the Jira instance.");
        Console.WriteLine("\t-JiraUserName, -u: The username for authenticating with Jira.");
        Console.WriteLine("\t-JiraToken, -k: The API token or password for authenticating with Jira.");
        Console.WriteLine("\t-SqlServerConnectionString, -s: The connection string for the SQL Server database.");
        Console.WriteLine("\t-DateForIncrementalUpdate, -d: The date for performing incremental updates in the ETL process.");
    }
}

internal class Arguments
{
    public string? JiraUrl { get; set; }
    public string? JiraUserName { get; set; }
    public string? JiraToken { get; set; }
    public string? SqlServerConnectionString { get; set; }
    public DateTime DateForIncrementalUpdate { get; set; }

    public static Arguments Parse(string[] args)
    {
        var arguments = new Arguments();

        for (int i = 0; i < args.Length; i += 2)
        {
            string argumentName = args[i].TrimStart('-');
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
                case "k":
                    arguments.JiraToken = argumentValue;
                    break;
                case "sqlserverconnectionstring":
                case "s":
                    arguments.SqlServerConnectionString = argumentValue;
                    break;
                case "dateforincrementalupdate":
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