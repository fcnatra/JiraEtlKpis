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
        SetupLogging();

        try
        {
            var arguments = Arguments.Parse(args);
            EtlProcess etl = new()
            {
                ExecutionArguments = arguments,
                JiraConnector = new JiraApiConnector
                {
                    JiraArguments = arguments
                }
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

    private static void SetupLogging()
    {
        AddLog4NetListener();

        // add console listener for debugging
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    private static void AddLog4NetListener()
    {
        XmlConfigurator.Configure();
        TextWriterTraceListener log4netListener = new TextWriterTraceListener(new Log4NetWriter());
        Trace.Listeners.Add(log4netListener);
    }
}
