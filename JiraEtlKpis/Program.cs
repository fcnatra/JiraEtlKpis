using System.Diagnostics;
using ETL;
using log4net;
using log4net.Config;
using Services;

internal class Program
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    
    private static void Main(string[] args)
    {
        SetupLogStrategy();

        try
        {
            var arguments = Arguments.Parse(args);
            var etl = new EtlProcess
            {
                ExecutionArguments = arguments,
                JiraConnector = new JiraApiConnector()
            };
            etl.Run();
        }
        catch (ArgumentException argEx)
        {
            log.Error($"\nERROR: {argEx.Message}");
            Console.WriteLine($"\nExecution error: {argEx.Message}\nCheck logs for more info.");
            Arguments.ShowHelp();
        }
        Console.WriteLine($"END.");
    }

    private static void SetupLogStrategy()
    {
        AddLog4NetTraceListener();

        // add console listener for debugging
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    private static void AddLog4NetTraceListener()
    {
        XmlConfigurator.Configure();
        TextWriterTraceListener log4netListener = new TextWriterTraceListener(new Log4NetWriter());
        Trace.Listeners.Add(log4netListener);
    }
}
