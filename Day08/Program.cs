using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

string title = "AdventOfCode2023 - Day 08";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

const string star1Filename = "input.txt";
const string star2Filename = "input.txt";

List<string> inputLines = [.. (await File.ReadAllLinesAsync(star1Filename))];

Stopwatch stopwatch = Stopwatch.StartNew();

string directions = null;

Regex nodeRegex = NodeRegex();

Dictionary<string, (string left, string right)> nodes = ParseNodes();

int star1 = 0;

List<string> currentNodes = ["AAA"];

int directionIndex = 0;

while (!currentNodes.All(x => x.EndsWith('Z')))
{
	bool left = directions[directionIndex] == 'L';

	for (int i = 0; i < currentNodes.Count; i++)
	{
		currentNodes[i] = left ? nodes[currentNodes[i]].left : nodes[currentNodes[i]].right;
	}

	directionIndex++;
	directionIndex %= directions.Length;

	star1++;
}

// Answer: 
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

inputLines = [.. (await File.ReadAllLinesAsync(star2Filename))];

nodes = ParseNodes();

currentNodes = nodes.Keys.Where(x => x.EndsWith('A')).ToList();

List<long> roundTotals = [];

for (int i = 0; i < currentNodes.Count; i++)
{
	roundTotals.Add(0);

	while (!currentNodes[i].EndsWith('Z'))
	{
		bool left = directions[directionIndex] == 'L';

		currentNodes[i] = left ? nodes[currentNodes[i]].left : nodes[currentNodes[i]].right;

		directionIndex++;
		directionIndex %= directions.Length;

		roundTotals[i]++;
	}
}

long star2 = CalculationHelper.LeastCommonDenominator(roundTotals);

// Answer: 10818234074807
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

void WriteNodes(List<string> currentNodes)
{
	if (currentNodes.Any(x => x.EndsWith('Z')))
	{
		foreach (string node in currentNodes)
		{
			ConsoleEx.Write(node, node.EndsWith('Z') ? ConsoleColor.Green : ConsoleColor.Yellow);
		}
	}
	else
	{
		ConsoleEx.Write(string.Join(string.Empty, currentNodes), ConsoleColor.Yellow);
	}

	Console.WriteLine();
}

Dictionary<string, (string left, string right)> ParseNodes()
{
	Dictionary<string, (string left, string right)> nodes = [];
	directions = null;
	directionIndex = 0;

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

	return nodes;
}

partial class Program
{
	[GeneratedRegex("(?<Node>\\w+)\\W+=\\W+(?<Left>\\w+),\\W?(?<Right>\\w+)")]
	private static partial Regex NodeRegex();
}