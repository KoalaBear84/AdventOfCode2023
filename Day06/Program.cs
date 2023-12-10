using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day 06";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

List<long> times = inputLines[0].Split(":").Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
List<long> distances = inputLines[1].Split(":").Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

int star1 = ProcessRaces();

// Answer: 160816
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

times = [long.Parse(string.Join(string.Empty, times))];
distances = [long.Parse(string.Join(string.Empty, distances))];

int star2 = ProcessRaces();

// Answer: 46561107
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

int ProcessRaces()
{
	int answer = -1;

	for (int i = 0; i < times.Count; i++)
	{
		long time = times[i];
		long distance = distances[i];

		int waysToWin = GetDistancesAbove(time, distance);

		if (answer == -1)
		{
			answer = waysToWin;
		}
		else
		{
			answer *= waysToWin;
		}
	}

	return answer;
}

int GetDistancesAbove(long raceTime, long distance)
{
	int waysToWin = 0;

	for (long time = 1; time < raceTime; time++)
	{
		long traveledDistance = (raceTime - time) * time;

		if (traveledDistance > distance)
		{
			waysToWin++;
		}
	}

	return waysToWin;
}