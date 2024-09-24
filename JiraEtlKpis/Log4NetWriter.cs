using System.Text;
using log4net;

public class Log4NetWriter : System.IO.TextWriter
{
    private readonly ILog log;

    public Log4NetWriter()
    {
        log = LogManager.GetLogger(typeof(Log4NetWriter));
    }

    public override void Write(char value)
    {
        // Aquí podrías implementar lógica adicional si es necesario.
    }

    public override void Write(string? value)
    {
        if (!string.IsNullOrEmpty(value))
            log.Info(value);
    }

    public override Encoding Encoding
    {
        get { return Encoding.UTF8; }
    }
}