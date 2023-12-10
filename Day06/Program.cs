using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day 06";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

List<int> times = inputLines[0].Split(":").Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
List<int> distances = inputLines[1].Split(":").Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

int star1 = -1;

for (int i = 0; i < times.Count; i++)
{
	int time = times[i];
	int distance = distances[i];

	int waysToWin = GetDistancesAbove(time, distance);

	if (star1 == -1)
	{
		star1 = waysToWin;
	} else
	{
		star1 *= waysToWin;
	}
}

int GetDistancesAbove(int raceTime, int distance)
{
	int waysToWin = 0;

	for (int time = 1; time < raceTime; time++)
	{
		int traveledDistance = (raceTime - time) * time;

		if (traveledDistance > distance)
		{
			waysToWin++;
		}
	}

	return waysToWin;
}

// Answer: 
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = -1;

// Answer: 
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

