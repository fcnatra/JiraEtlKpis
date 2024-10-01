using ETL.Interfaces;
using log4net;

namespace ETL;

public class EtlProcess
{
    public Arguments? ExecutionArguments { get; set; }
    public IJiraConnector? JiraConnector { get; set; }

    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    
    public EtlProcess()
    {
    }

    public void Run()
    {
        CheckArgumentsAreProvided();
        CheckConnectorsAreProvided();
        
        log.Info($"Starting ETL process with args: \"{ExecutionArguments?.JiraUrl}\" \"{ExecutionArguments?.JiraUserName}\" \"TOKEN_HIDDEN_FOR_PRIVACY\" \"{ExecutionArguments?.DateForIncrementalUpdate}\"");

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
        
        this.JiraConnector.BlockSize = 2000;
    }

    public static void RunWith(Arguments arguments)
    {
        EtlProcess etl = new();
        etl.ExecutionArguments = arguments;
        etl.Run();
    }
}
