using ETL.Interfaces;
using log4net;
using System.Diagnostics;

namespace ETL;

public class EtlProcess
{
    public Arguments? ExecutionArguments { get; set; }
    public IJiraConnector? JiraConnector { get; set; }

    private static readonly ILog log = LogManager.GetLogger(typeof(Program));

    public void Run()
    {
        var executionArguments = CheckArgumentsAreProvided();
        var jiraConnector = CheckJiraConnectorIsProvided();

        jiraConnector.JiraArguments = ExecutionArguments;

        log.Info($"Starting ETL process with args: \"{executionArguments.Url}\" \"TOKEN_HIDDEN_FOR_PRIVACY\" \"{executionArguments.DateForIncrementalUpdate}\"");

        ProcessIssues(executionArguments, jiraConnector);

        log.Info($"Process ENDED");
    }

    private IJiraConnector CheckJiraConnectorIsProvided()
    {
        if (JiraConnector is null) throw new InvalidOperationException("Jira Connector not provided");
        return JiraConnector;
    }

    private Arguments CheckArgumentsAreProvided()
    {
        if (this.ExecutionArguments is null) throw new ArgumentException($"Arguments not provided");
        return ExecutionArguments;
    }

    private void ProcessIssues(Arguments arguments, IJiraConnector jiraConnector)
    {        
        Debug.WriteLine(jiraConnector.GetIssuesSince(arguments.DateForIncrementalUpdate));
    }
}
