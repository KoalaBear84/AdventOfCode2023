using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

string title = "AdventOfCode2023 - Day 08";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

string directions = null;

Regex nodeRegex = NodeRegex();

Dictionary<string, (string left, string right)> nodes = [];

foreach (string input in inputLines)
{
	if (directions is null)
	{
		directions = input;
		continue;
	}

	if (string.IsNullOrWhiteSpace(input))
	{
		continue;
	}

	Match regexMatch = nodeRegex.Match(input);

	if (regexMatch.Success)
	{
		string node = regexMatch.Groups["Node"].Value;
		string left = regexMatch.Groups["Left"].Value;
		string right = regexMatch.Groups["Right"].Value;

		nodes[node] = (left, right);
	}
}

int star1 = 0;

string currentNode = "AAA";
int directionIndex = 0;

while (currentNode != "ZZZ")
{
	bool left = directions[directionIndex] == 'L';

	currentNode = left ? nodes[currentNode].left : nodes[currentNode].right;

	directionIndex++;
	directionIndex %= directions.Length;

	star1++;
}

// Answer: 
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = -1;

// Answer: 
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

partial class Program
{
	[GeneratedRegex("(?<Node>\\w+)\\W+=\\W+(?<Left>\\w+),\\W?(?<Right>\\w+)")]
	private static partial Regex NodeRegex();
}