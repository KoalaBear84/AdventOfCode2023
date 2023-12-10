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

int star1 = -1;

// Answer: 
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = -1;

// Answer: 
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

