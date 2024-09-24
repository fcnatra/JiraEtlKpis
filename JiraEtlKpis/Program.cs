using System.Diagnostics;
using log4net;
using log4net.Config;

internal class Program
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    
    private static void Main(string[] args)
    {
        AddTraceListenerForLogging();
        Trace.Listeners.Add(new ConsoleTraceListener());

        try
        {
            var arguments = Arguments.Parse(args);

            log.Info($"{arguments.DateForIncrementalUpdate}");

            var etlProcess = new EtlProcess();
            etlProcess.RunWith(arguments);
        }
        catch (ArgumentException argEx)
        {
            log.Error($"\nERROR: {argEx.Message}");
            Arguments.ShowHelp();
        }
    }

    private static void AddTraceListenerForLogging()
    {
        XmlConfigurator.Configure();
        TextWriterTraceListener log4netListener = new TextWriterTraceListener(new Log4NetWriter());
        Trace.Listeners.Add(log4netListener);
    }
}
