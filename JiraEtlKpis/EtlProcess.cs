
using log4net;

internal class EtlProcess
{
    public Arguments? ExecutionArguments { get; internal set; }
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    
    public EtlProcess()
    {
    }

    internal void Run()
    {
        if (this.ExecutionArguments is null) throw new ArgumentException($"Arguments not provided");
    }

    internal static void RunWith(Arguments arguments)
    {
        EtlProcess etl = new();
        etl.ExecutionArguments = arguments;
        etl.Run();
    }
}