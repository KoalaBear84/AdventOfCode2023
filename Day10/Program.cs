using Library;
using QuikGraph;
using System.Diagnostics;
using System.Drawing;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Search;

string title = "AdventOfCode2023 - Day 10";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

char[,] grid = new char[inputLines[0].Length, inputLines.Count];
int[,] distances = new int[inputLines[0].Length, inputLines.Count];

const short Dimension_X = 0;
const short Dimension_Y = 1;

for (int x = 0; x <= distances.GetUpperBound(Dimension_X); x++)
{
	for (int y = 0; y <= distances.GetUpperBound(Dimension_Y); y++)
	{
		distances[x, y] = -1;
	}
}

Point startPosition = new();

for (int y = 0; y < inputLines.Count; y++)
{
	for (int x = 0; x < inputLines[y].Length; x++)
	{
		char c = inputLines[y][x];
		grid[x, y] = c;

		if (c == 'S')
		{
			startPosition = new Point(x, y);
		}
	}
}

UndirectedGraph<Point, Edge<Point>> g = new();

for (int y = 0; y <= grid.GetUpperBound(Dimension_Y); y++)
{
	for (int x = 0; x <= grid.GetUpperBound(Dimension_X); x++)
	{
		Point point = new(x, y);

		IEnumerable<Direction> directions = GetDirections(point);

		foreach (Direction direction in directions)
		{
			Point newPoint = direction switch
			{
				Direction.North => new(point.X, point.Y - 1),
				Direction.East => new(point.X + 1, point.Y),
				Direction.South => new(point.X, point.Y + 1),
				Direction.West => new(point.X - 1, point.Y),
				_ => throw new NotImplementedException()
			};

			g.AddVerticesAndEdge(new Edge<Point>(point, newPoint));
		}
	}
}

// Save to Graphviz file (Needs QuickGraph.Graphviz nuget)
//File.WriteAllText("GraphViz.gv", g.ToGraphviz());

UndirectedBreadthFirstSearchAlgorithm<Point, Edge<Point>> algorithm = new(g);
algorithm.Compute(startPosition);

// More debugging tools
//UndirectedVertexPredecessorRecorderObserver<Point, Edge<Point>> observer = new();

//using (observer.Attach(algorithm))
//{
//	algorithm.Compute(startPosition);
//}

int star1 = -1;

Func<Edge<Point>, double> edgeCost = edge => 1; // Constant cost

// Compute shortest paths
TryFunc<Point, IEnumerable<Edge<Point>>> tryGetPaths = g.ShortestPathsDijkstra(edgeCost, startPosition);

for (int y = 0; y <= distances.GetUpperBound(Dimension_Y); y++)
{
	for (int x = 0; x <= distances.GetUpperBound(Dimension_X); x++)
	{
		Point point = new(x, y);

		KeyValuePair<Point, GraphColor> verticeColor = algorithm.VerticesColors.FirstOrDefault(x => point.X == x.Key.X && point.Y == x.Key.Y);

		bool isConnected = verticeColor.Value == GraphColor.Black;

		if (tryGetPaths(point, out IEnumerable<Edge<Point>>? path))
		{
			int distance = path.Count();
			distances[x, y] = distance;
			star1 = Math.Max(distance, star1);
		}
	}
}

DrawGrid(star1);
//DrawDistances();

// Answer: 6931
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = -1;

// Answer: 
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

IEnumerable<Direction> GetDirections(Point point)
{
	// ╚ L
	// ╔ F
	// ╝ J
	// ╗ 7
	// ═ -
	// ║ |

	char source = grid[point.X, point.Y];

	if (point.Y > 0)
	{
		char target = grid[point.X, point.Y - 1];

		if (source is 'S' or '|' or 'L' or 'J' &&
			target is 'S' or '|' or 'F' or '7')
		{
			yield return Direction.North;
		}
	}

	if (point.X > 0)
	{
		char target = grid[point.X - 1, point.Y];

		if (source is 'S' or '-' or 'J' or '7' &&
			target is 'S' or '-' or 'F' or 'L')
		{
			yield return Direction.West;
		}
	}

	if (point.X < grid.GetUpperBound(0) - 1)
	{
		char target = grid[point.X + 1, point.Y];

		if (source is 'S' or '-' or 'L' or 'F' &&
			target is 'S' or '-' or 'J' or '7')
		{
			yield return Direction.East;
		}
	}

	if (point.Y < grid.GetUpperBound(1) - 1)
	{
		char target = grid[point.X, point.Y + 1];

		if (source is 'S' or '|' or 'F' or '7' &&
			target is 'S' or '|' or 'L' or 'J')
		{
			yield return Direction.South;
		}
	}
}

Direction GetDirection(Direction source, Point point)
{
	return GetDirections(point).Where(x => x != source).FirstOrDefault();
}

void DrawGrid(int maxDistance)
{
	for (int y = 0; y <= grid.GetUpperBound(Dimension_Y); y++)
	{
		for (int x = 0; x <= grid.GetUpperBound(Dimension_X); x++)
		{
			int distance = distances[x, y];
			char c = grid[x, y];

			if (distance == maxDistance || c == 'S')
			{
				ConsoleEx.WriteRgb($"{ConvertChar(c)}", Color.Red);
				continue;
			}

			if (distance == -1)
			{
				Console.Write(ConvertChar(c));
				continue;
			}

			int green = (int)(200d / maxDistance * distance) + 55;

			ConsoleEx.WriteRgb($"{ConvertChar(c)}", Color.FromArgb(0, green, 0));
		}

		Console.WriteLine();
	}
}

void DrawDistances()
{
	int max = 0;

	for (int y = 0; y <= distances.GetUpperBound(Dimension_Y); y++)
	{
		for (int x = 0; x <= distances.GetUpperBound(Dimension_X); x++)
		{
			max = Math.Max(distances[x, y], max);
		}
	}

	for (int y = 0; y <= distances.GetUpperBound(Dimension_Y); y++)
	{
		for (int x = 0; x <= distances.GetUpperBound(Dimension_X); x++)
		{
			if (distances[x, y] == -1)
			{
				Console.Write('.');
				continue;
			}

			Console.Write($"{distances[x, y]}");
		}

		Console.WriteLine();
	}
}

static string ConvertChars(string input)
{
	return new string(input.Select(ConvertChar).ToArray());
}

static char ConvertChar(char c)
{
	return c switch
	{
		'.' => '.',
		'-' => '═',
		'|' => '║',
		'F' => '╔',
		'7' => '╗',
		'J' => '╝',
		'L' => '╚',
		'S' => 'S',
		_ => ' '
	};
}

enum Direction
{
	North,
	East,
	South,
	West
}
