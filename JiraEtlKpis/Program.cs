using System.Buffers;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var arguments = Arguments.Parse(args);
        }
        catch (ArgumentException argEx)
        {
            Console.WriteLine($"\nERROR: {argEx.Message}");
            Arguments.ShowHelp();
        }
    }
}
