using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day 03";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

char[,] grid = new char[inputLines.Count, inputLines[0].Length];

Stopwatch stopwatch = Stopwatch.StartNew();

for (int y = 0; y < inputLines.Count; y++)
{
	string input = inputLines[y];

	for (int x = 0; x < input.Length; x++)
	{
		char c = input[x];

		grid[y, x] = c;
	}
}

int width = grid.GetUpperBound(1);
int height = grid.GetUpperBound(0);

//DrawGrid(grid);

List<int> validParts = [];

for (int y = 0; y <= height; y++)
{
	string numberString = string.Empty;
	int number = 0;
	bool hasSymbol = false;

	for (int x = 0; x <= width; x++)
	{
		char c = grid[y, x];

		bool isNumber = char.IsNumber(c);

		if (isNumber)
		{
			numberString += c;

			// Get neighbours, check symbols
			hasSymbol |= neighboursHaveSymbol(x, y);
		}

		if (!isNumber || x == height)
		{
			if (numberString != string.Empty)
			{
				number = int.Parse(numberString);

				if (hasSymbol)
				{
					//ConsoleEx.WriteLine(numberString, ConsoleColor.Green);
					validParts.Add(number);
				}
				else
				{
					//ConsoleEx.WriteLine(numberString, ConsoleColor.Red);
				}

				hasSymbol = false;
			}

			numberString = string.Empty;
		}
	}
}

bool neighboursHaveSymbol(int x, int y)
{
	// Top
	if (y > 0 && isSymbol(grid[y - 1, x]))
	{
		return true;
	}

	// Top right
	if (x < width && y > 0 && isSymbol(grid[y - 1, x + 1]))
	{
		return true;
	}

	// Right
	if (x < width && isSymbol(grid[y, x + 1]))
	{
		return true;
	}

	// Bottom right
	if (x < width && y < height && isSymbol(grid[y + 1, x + 1]))
	{
		return true;
	}

	// Bottom
	if (y < height && isSymbol(grid[y + 1, x]))
	{
		return true;
	}

	// Bottom left
	if (x > 0 && y < height && isSymbol(grid[y + 1, x - 1]))
	{
		return true;
	}

	// Left
	if (x > 0 && isSymbol(grid[y, x - 1]))
	{
		return true;
	}

	// Top left
	if (x > 0 && y > 0 && isSymbol(grid[y - 1, x - 1]))
	{
		return true;
	}

	return false;
}

bool isSymbol(char c)
{
	return c != '.' && !char.IsNumber(c);
}


// Answer: 535351
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {validParts.Sum()}", ConsoleColor.Yellow);

stopwatch.Restart();

// Answer: 
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: ", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

void DrawGrid(char[,] grid)
{
	for (int y = 0; y <= height; y++)
	{
		for (int x = 0; x <= width; x++)
		{
			Console.Write(grid[y, x]);
		}

		Console.WriteLine();
	}
}