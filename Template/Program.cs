using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day XX";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("inputtest.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

foreach (string input in inputLines)
{

}

// Answer: 
ConsoleEx.WriteLine($"Star 1. {(double)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond:n2}ms. Answer: ", ConsoleColor.Yellow);

stopwatch.Restart();

// Answer: 
ConsoleEx.WriteLine($"Star 2. {(double)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond:n2}ms. Answer: ", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();