using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

string title = "AdventOfCode2023 - Day 02";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

Regex gameRegex = GameRegex();

Dictionary<string, int> maxDisplays = new()
{
	{ "red", 12 },
	{ "green", 13 },
	{ "blue", 14 }
};

Dictionary<int, Game> games = [];

foreach (string input in inputLines)
{
	Match gameMatch = gameRegex.Match(input);
	int gameNumber = int.Parse(gameMatch.Groups["Game"].Value);

	Game game = new();

	foreach (string displays in gameMatch.Groups["Displays"].Value.Split(";"))
	{
		Display display = new();

		foreach (string displayString in displays.Split(","))
		{
			Match showMatch = ShowRegex().Match(displayString);

			string color = showMatch.Groups["Color"].Value;
			int amount = int.Parse(showMatch.Groups["Amount"].Value);

			display.Colors[color] = amount;
		}

		game.Displays.Add(display);
	}

	games[gameNumber] = game;
}

IEnumerable<KeyValuePair<int, Game>> validGamesStar1 = games.Where(x => x.Value.Displays.All(x => x.Colors.All(x => maxDisplays[x.Key] >= x.Value)));

// Answer: 2449
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {validGamesStar1.Select(x => x.Key).Sum()}", ConsoleColor.Yellow);

int star2 = games
		.Select(x => x.Value.Displays.SelectMany(x => x.Colors)
		.GroupBy(x => x.Key))
		.Select(x => x.Select(x => x.Max(x => x.Value)))
		.Select(x => x.Aggregate((x, y) => x * y)).Sum();

stopwatch.Restart();

// Answer: 63981
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

partial class Program
{
	[GeneratedRegex("Game (?<Game>\\d+):(?<Displays>.*)")]
	private static partial Regex GameRegex();

	[GeneratedRegex("(?:(?<Amount>\\d+) (?<Color>\\w+))")]
	private static partial Regex ShowRegex();
}

public class Game
{
	public List<Display> Displays { get; set; } = [];
}

public class Display
{
	public Dictionary<string, int> Colors { get; set; } = [];
}