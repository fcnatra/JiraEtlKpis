using ETL.Interfaces;
using log4net;
using System.Diagnostics;

namespace ETL;

public class EtlProcess
{
    public Arguments ExecutionArguments { get; set; }
    public IJiraConnector? JiraConnector { get; set; }

    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    
    public EtlProcess(Arguments arguments)
    {
        ExecutionArguments = arguments;
    }

    public static void RunWith(Arguments arguments)
    {
        EtlProcess etl = new(arguments);
        etl.Run();
    }

    public void Run()
    {
        CheckArgumentsAreProvided();
        CheckConnectorsAreProvided();
        
        log.Info($"Starting ETL process with args: \"{ExecutionArguments?.Url}\" \"TOKEN_HIDDEN_FOR_PRIVACY\" \"{ExecutionArguments?.DateForIncrementalUpdate}\"");

        ProcessIssues();

        log.Info($"Process ENDED");
    }

    private void CheckConnectorsAreProvided()
    {
        if (JiraConnector is null) throw new InvalidOperationException("Jira Connector not provided");
    }

    private void CheckArgumentsAreProvided()
    {
        if (this.ExecutionArguments is null) throw new ArgumentException($"Arguments not provided");
    }

    private void ProcessIssues()
    {
        if (JiraConnector is null) return;
        
        Debug.WriteLine(JiraConnector.GetIssuesSince(ExecutionArguments.DateForIncrementalUpdate));
    }
}
