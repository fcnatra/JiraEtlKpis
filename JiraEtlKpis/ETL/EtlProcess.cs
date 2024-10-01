
using System.Configuration;
using ETL.Interfaces;
using log4net;

namespace ETL
{
    internal class EtlProcess
    {
        public Arguments? ExecutionArguments { get; internal set; }
        public IJiraConnector? JiraConnector { get; private set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        
        public EtlProcess()
        {
        }

        internal void Run()
        {
            CheckArguments();
            CheckConnectors();
            
            log.Info($"Starting ETL process with args: \"{ExecutionArguments?.JiraUrl}\" \"{ExecutionArguments?.JiraUserName}\" \"TOKEN_HIDDEN_FOR_PRIVACY\" \"{ExecutionArguments?.DateForIncrementalUpdate}\"");

            ProcessIssues();

            log.Info($"Process ENDED");
        }

        private void CheckConnectors()
        {
            if (JiraConnector is null) throw new Exception("Jira Connector not provided");
        }

        private void CheckArguments()
        {
            if (this.ExecutionArguments is null) throw new ArgumentException($"Arguments not provided");
        }

        private void ProcessIssues()
        {
            if (JiraConnector is null) return;
            
            this.JiraConnector.BlockSize = 2000;
        }

        internal static void RunWith(Arguments arguments)
        {
            EtlProcess etl = new();
            etl.ExecutionArguments = arguments;
            etl.Run();
        }
    }
}