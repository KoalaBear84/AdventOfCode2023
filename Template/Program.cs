using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day XX";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("inputtest.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

foreach (string input in inputLines)
{

}

// Answer: 
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: ", ConsoleColor.Yellow);

stopwatch.Restart();

// Answer: 
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: ", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();