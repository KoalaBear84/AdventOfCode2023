using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day 09";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

List<List<int>> numbers = [];

foreach (string input in inputLines)
{
	numbers.Add(input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList());
}

int star1 = numbers.Select(GetExtrapolatedValueForward).Sum();

// Answer: 1992273652
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = numbers.Select(GetExtrapolatedValueBackward).Sum();

// Answer: 769450174 (Too High)
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

// Might not be the fastest, but it's OK
static int GetExtrapolatedValueForward(List<int> puzzle)
{
	List<List<int>> answer = [];

	answer.Add(puzzle);

	while (!answer.Last().All(x => x == 0))
	{
		List<int> innerAnswer = [];

		List<int> list = answer.Last();

		for (int i = 0; i < list.Count - 1; i++)
		{
			innerAnswer.Add(list[i + 1] - list[i]);
		}

		answer.Add(innerAnswer);
	}

	for (int i = answer.Count - 2; i >= 0; i--)
	{
		answer[i].Add(answer[i + 1].Last() + answer[i].Last());
	}

	return answer[0].Last();
}

static int GetExtrapolatedValueBackward(List<int> puzzle)
{
	List<List<int>> answer = [];

	answer.Add(puzzle);

	while (!answer.Last().All(x => x == 0))
	{
		List<int> innerAnswer = [];

		List<int> list = answer.Last();

		for (int i = 0; i < list.Count - 1; i++)
		{
			innerAnswer.Add(list[i + 1] - list[i]);
		}

		answer.Add(innerAnswer);
	}

	for (int i = answer.Count - 2; i >= 0; i--)
	{
		answer[i].Insert(0, answer[i].First() - answer[i + 1].First());
	}

	return answer[0].First();
}