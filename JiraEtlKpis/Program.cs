using System.Diagnostics;
using log4net;
using log4net.Config;

internal class Program
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    
    private static void Main(string[] args)
    {
        AddTraceListeners();

        try
        {
            var arguments = Arguments.Parse(args);

            log.Info($"{arguments.DateForIncrementalUpdate}");
            EtlProcess.RunWith(arguments);
        }
        catch (ArgumentException argEx)
        {
            log.Error($"\nERROR: {argEx.Message}");
            Arguments.ShowHelp();
        }
    }

    private static void AddTraceListeners()
    {
        AddLog4NetListener();

        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    private static void AddLog4NetListener()
    {
        XmlConfigurator.Configure();
        TextWriterTraceListener log4netListener = new TextWriterTraceListener(new Log4NetWriter());
        Trace.Listeners.Add(log4netListener);
    }
}
