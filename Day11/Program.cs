using Library;
using System.Diagnostics;
using System.Numerics;

string title = "AdventOfCode2023 - Day 11";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

int width = inputLines[0].Length;
int height = inputLines.Count;

List<Vector2> galaxies = ReadGalaxies(width, height);

//DrawGalaxies(galaxies);
ExpandUniverse(galaxies, width, height);

List<long> distances = GetDistances();

long star1 = distances.Sum();

// Answer: 10228230
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

galaxies = ReadGalaxies(width, height);
ExpandUniverse(galaxies, width, height, 1_000_000);

distances = GetDistances();

long star2 = distances.Sum();

// Answer: 447073334102
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

void ExpandUniverse(List<Vector2> galaxies, int width, int height, int expansion = 2)
{
	List<int> emptyCols = Enumerable.Range(0, width + 1).Except(galaxies.GroupBy(galaxy => galaxy.X).Select(group => (int)group.Key)).ToList();
	List<int> emptyRows = Enumerable.Range(0, height + 1).Except(galaxies.GroupBy(galaxy => galaxy.Y).Select(group => (int)group.Key)).ToList();

	for (int i = 0; i < galaxies.Count; i++)
	{
		Vector2 galaxy = galaxies[i];

		int addCols = emptyCols.Count(x => x < galaxy.X);
		int addRows = emptyRows.Count(x => x < galaxy.Y);

		if (addCols > 0 || addRows > 0)
		{
			//Console.WriteLine($"{galaxy.X},{galaxy.Y} => {(galaxy.X + addCols)},{(galaxy.Y + addRows)}");

			galaxy.X += addCols * (expansion - 1);
			galaxy.Y += addRows * (expansion - 1);

			galaxies[i] = galaxy;
		}
	}
}

void DrawGalaxies(List<Vector2> galaxies)
{
	int width = (int)galaxies.Max(galaxy => galaxy.X);
	int height = (int)galaxies.Max(galaxy => galaxy.Y);

	for (int y = 0; y <= height; y++)
	{
		for (int x = 0; x <= width; x++)
		{
			char c = galaxies.Any(galaxy => galaxy.X == x && galaxy.Y == y) ? '#' : '.';
			Console.Write(c);
		}

		Console.WriteLine();
	}
}

List<Vector2> ReadGalaxies(int width, int height)
{
	List<Vector2> galaxies = [];

	for (int y = 0; y < height; y++)
	{
		for (int x = 0; x < width; x++)
		{
			char c = inputLines[y][x];

			if (c is '#')
			{
				galaxies.Add(new Vector2(x, y));
			}
		}
	}

	return galaxies;
}

List<long> GetDistances()
{
	List<long> distances = [];

	for (int i = 0; i < galaxies.Count; i++)
	{
		for (int j = i + 1; j < galaxies.Count; j++)
		{
			Vector2 galaxy1 = galaxies[i];
			Vector2 galaxy2 = galaxies[j];
			Vector2 diff = galaxy2 - galaxy1;

			long distance = Math.Abs((long)diff.X) + Math.Abs((long)diff.Y);

			//Console.WriteLine($"#{i + 1} {galaxy1} -> #{j + 1} {galaxy2}, {distance}");

			distances.Add(distance);
		}
	}

	return distances;
}