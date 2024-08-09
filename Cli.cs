namespace Exercise2;

public static class Cli
{
    public static void PrintLine(string message, ConsoleColor colorText = ConsoleColor.White, ConsoleColor colorBackground = ConsoleColor.Black)
    {
        Console.ForegroundColor = colorText;
        Console.BackgroundColor = colorBackground;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintError(string message)
    {
        PrintLine(message, ConsoleColor.Red);
    }

    public static void PrintStandartLine(string message)
    {
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
    
    public static void PrintWarning(string message)
    {
        PrintLine(message, ConsoleColor.Yellow);
        Console.ResetColor();
    }

    public static void PrintMenu()
    {
        Console.Clear();
        Thread.Sleep(250);
        PrintLine("\n\t\t\tWelcome to the Coffee Machine!\n", ConsoleColor.Magenta);
        Thread.Sleep(200);
        PrintLine("\n\tMenu\n");
        Thread.Sleep(200);
        PrintLine("\t1.Brew coffee (Espresso/Cappuccino)");
        Thread.Sleep(200);
        PrintLine("\t2.Check ingredients");
        Thread.Sleep(200);
        PrintLine("\t3. Profiles");
        Thread.Sleep(200);
        PrintLine("\t4. Recipes");
        Thread.Sleep(200);
        PrintLine("\t5. Exit");
        Thread.Sleep(200);
        PrintLine("\n\t6. Log");
    }

    public static void PrintSucces(string message)
    {
        PrintLine(message, ConsoleColor.Green);
        Console.ResetColor();
    }

    public static void Animation(string message, int count)
    {
        int completedCounts = 0;
        do
        {
            Console.Clear();
            Thread.Sleep(500);
            PrintLine($"{message}", ConsoleColor.Yellow);
            Thread.Sleep(500);
            Console.Clear();
            PrintLine($"{message}.", ConsoleColor.Yellow);
            Thread.Sleep(500);
            Console.Clear();
            PrintLine($"{message}..", ConsoleColor.Yellow);
            Thread.Sleep(500);
            Console.Clear();
            PrintLine($"{message}...", ConsoleColor.Yellow);
            Thread.Sleep(500);
            completedCounts++;
        } while (completedCounts < count);
    }
}
