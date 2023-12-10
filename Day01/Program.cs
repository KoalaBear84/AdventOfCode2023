using Library;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

string title = "AdventOfCode2023 - Day 01";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

int star1 = CalculateSum(inputLines);

// Answer: 54388
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

Regex star2Regex = Star2Regex();

// SPOILER: 'twone' -> '21' needs to also be supported

for (int i = 0; i < inputLines.Count; i++)
{
	// Sadly Regex.Replace doesn't really work with lookaheads so instead it 'adds' all the results
	inputLines[i] = star2Regex.Replace(inputLines[i], match => string.Join(string.Empty, match.Groups.Values.Select(group => group.Value switch
	{
		"one" => "1",
		"two" => "2",
		"three" => "3",
		"four" => "4",
		"five" => "5",
		"six" => "6",
		"seven" => "7",
		"eight" => "8",
		"nine" => "9",
		_ => string.Empty,
	})));
}

int star2 = CalculateSum(inputLines);

// Answer: 53515
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

static int CalculateSum(List<string> inputLines)
{
	int sum = 0;

	foreach (string input in inputLines)
	{
		int? firstDigit = null;
		int? lastDigit = null;

		foreach (char c in input)
		{
			if (!char.IsDigit(c))
			{
				continue;
			}

			int digit = CharUnicodeInfo.GetDecimalDigitValue(c);
			firstDigit ??= digit;
			lastDigit = digit;
		}

		int add = (10 * firstDigit.Value) + lastDigit.Value;
		sum += add;
	}

	return sum;
}

partial class Program
{
	[GeneratedRegex("(?=(one|two|three|four|five|six|seven|eight|nine))")]
	private static partial Regex Star2Regex();
}