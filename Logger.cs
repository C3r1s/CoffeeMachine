namespace Exercise2;

using static Cli;

public class Logger
{
   public List<string?> Log { get; } = new List<string?>();
private readonly string _logDirectoryPath = Path.Combine(Environment.CurrentDirectory, "logs");
private static string? _logFilePath;
private static string _logFileName = $"coffee_machine_log {DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

public void LogWrite(string operation)
{
    Log.Add($"{DateTime.Now}: {operation}");
    string result = $"{DateTime.Now}: {operation}\n";
    SaveToFile(result);
}
    

    public void ShowLog()
    {
        Console.Clear();
        PrintLine("---------LOG---------", ConsoleColor.DarkRed);
        Log.ForEach(s => PrintLine(s, ConsoleColor.DarkRed));
        string choice = Console.ReadLine()!;
    }

    public void SaveToFile(string log)
    {
        Directory.CreateDirectory(_logDirectoryPath);
        _logFilePath = Path.Combine(_logDirectoryPath, _logFileName);
        using (StreamWriter writer = new StreamWriter(_logFilePath, true))
        {
            writer.WriteLine(log);
        }
    }
}