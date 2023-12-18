using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day 12";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("inputtest.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

int star1 = 0;
long totalPosibilities = 0;

foreach (string input in inputLines)
{
	star1 += CheckPossibilities(input);
}

// Answer: 7653
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = 0;

foreach (string input in inputLines)
{
	star2 += CheckPossibilities(input, true);
}

// Answer: 
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

int CheckPossibilities(string input, bool star2 = false)
{
	string[] splitted = input.Split(' ');

	if (star2)
	{
		splitted[0] = string.Join('?', Enumerable.Repeat(splitted[0], 5));
		splitted[1] = string.Join(',', Enumerable.Repeat(splitted[1], 5));
	}

	string hotSprings = splitted[0];
	List<int> groupCounts = splitted[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

	List<int> questionIndexes = GetIndexes(splitted[0], '?');

	if (questionIndexes.Count == 0)
	{
		return groupCounts.SequenceEqual(GetGroupCounts(hotSprings)) ? 1 : 0;
	}

	char[] hotSpringsChar = hotSprings.ToCharArray();
	int groupPossibilities = 0;

	long possibilities = (long)Math.Pow(2, questionIndexes.Count);

	totalPosibilities += possibilities;

	for (int possibility = 0; possibility < possibilities; possibility++)
	{
		for (int questionIndex = 0; questionIndex < questionIndexes.Count; questionIndex++)
		{
			hotSpringsChar[questionIndexes[questionIndex]] = IsBitSet(possibility, questionIndex) ? '#' : '.';
		}

		// Debugging
		//for (int x = 0; x < hotSpringsChar.Length; x++)
		//{
		//	if (questionIndexes.Contains(x))
		//	{
		//		ConsoleEx.WriteRgb(hotSpringsChar[x], Color.Yellow);
		//	}
		//	else
		//	{
		//		Console.Write(hotSpringsChar[x]);
		//	}
		//}

		//Console.WriteLine($" -> {splitted[1]}");

		if (groupCounts.SequenceEqual(GetGroupCountsChar(hotSpringsChar)))
		//if (groupCounts.SequenceEqual(GetGroupCounts(new string(hotSpringsChar))))
		{
			groupPossibilities++;
		}
	}

	return groupPossibilities;
}

List<int> GetGroupCounts(string input)
{
	if (input.All(x => x is '#'))
	{
		return [input.Length];
	}

	return input.Split('.', StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('#')).Select(x => x.Length).ToList();
}

List<int> GetGroupCountsChar(char[] input)
{
	if (input.All(x => x is '#'))
	{
		return [input.Length];
	}

	int count = 0;
	List<int> counts = [];

	for (int i = 0; i < input.Length; i++)
	{
		if (input[i] is '#')
		{
			count++;
		}
		else if (count > 0)
		{
			counts.Add(count);
			count = 0;
		}
	}

	if (count > 0)
	{
		counts.Add(count);
	}

	return counts;
}

List<int> GetIndexes(string input, char c)
{
	List<int> indexes = [];

	for (int i = 0; i < input.Length; i++)
	{
		if (input[i] == c)
		{
			indexes.Add(i);
		}
	}

	return indexes;
}

static bool IsBitSet(int number, int pos)
{
	return (number & (1 << pos)) > 0;
}
