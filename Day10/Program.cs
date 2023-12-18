using Library;
using QuikGraph;
using System.Diagnostics;
using System.Drawing;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Observers;

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

UndirectedGraph<Point, Edge<Point>> graph = new();
HashSet<Point> enclosedPoints = [];

for (int y = 0; y <= grid.GetUpperBound(Dimension_Y); y++)
{
	for (int x = 0; x <= grid.GetUpperBound(Dimension_X); x++)
	{
		Point point = new(x, y);
		enclosedPoints.Add(point);

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

			graph.AddVerticesAndEdge(new Edge<Point>(point, newPoint));
		}
	}
}

// Save to Graphviz file (Needs QuickGraph.Graphviz nuget)
//File.WriteAllText("GraphViz.gv", graph.ToGraphviz());

UndirectedBreadthFirstSearchAlgorithm<Point, Edge<Point>> algorithm = new(graph);
algorithm.Compute(startPosition);

// More debugging tools
UndirectedVertexPredecessorRecorderObserver<Point, Edge<Point>> observer = new();

using (observer.Attach(algorithm))
{
	algorithm.Compute(startPosition);
}

int star1 = -1;

Func<Edge<Point>, double> edgeCost = edge => 1; // Constant cost

// Compute shortest paths
TryFunc<Point, IEnumerable<Edge<Point>>> tryGetPaths = graph.ShortestPathsDijkstra(edgeCost, startPosition);

Dictionary<string, GraphColor> colors = algorithm.VerticesColors.Select(x => ($"{x.Key.X},{x.Key.Y}", x.Value)).ToDictionary();

for (int y = 0; y <= distances.GetUpperBound(Dimension_Y); y++)
{
	for (int x = 0; x <= distances.GetUpperBound(Dimension_X); x++)
	{
		Point point = new(x, y);

		colors.TryGetValue($"{point.X},{point.Y}", out GraphColor verticeColor);

		bool isConnected = verticeColor == GraphColor.Black;

		if (!isConnected)
		{
			graph.RemoveVertex(point);
			continue;
		}

		if (tryGetPaths(point, out IEnumerable<Edge<Point>>? path))
		{
			int distance = path.Count();
			distances[x, y] = distance;
			star1 = Math.Max(distance, star1);
		}
	}
}

Point currentPoint = graph.Vertices.OrderBy(vertice => vertice.Y).ThenBy(vertice => vertice.X).First();
Direction currentDirection = Direction.East;

DrawGrid(star1);
//DrawDistances();

// Answer: 6931
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

// Code for star 2 went mental

HashSet<Point> visitedPoints = [currentPoint];

int connectedNodes = colors.Where(x => x.Value == GraphColor.Black).Count();

CheckEnclosedPoints(ConvertStart('S', currentPoint), false);

while (visitedPoints.Count != graph.VertexCount)
{
	//char c = grid[currentPoint.X, currentPoint.Y];

	List<Direction> possibleDirections = GetDirections(currentPoint).Where(direction =>
		currentDirection is Direction.North && direction is Direction.North or Direction.East ||
		currentDirection is Direction.East && direction is Direction.East or Direction.West ||
		currentDirection is Direction.South && direction is Direction.South or Direction.West ||
		currentDirection is Direction.West && direction is Direction.West or Direction.North
	).ToList();

	Point loopPoint = currentPoint;

	foreach (Direction direction in possibleDirections)
	{
		Point point = direction switch
		{
			Direction.North => new(loopPoint.X, loopPoint.Y - 1),
			Direction.East => new(loopPoint.X + 1, loopPoint.Y),
			Direction.South => new(loopPoint.X, loopPoint.Y + 1),
			Direction.West => new(loopPoint.X - 1, loopPoint.Y),
			_ => throw new NotImplementedException()
		};

		if (visitedPoints.Contains(point))
		{
			continue;
		}

		visitedPoints.Add(point);

		currentPoint = point;

		// Debugging
		//Console.WriteLine(currentDirection);
		//DrawGrid(star1, point);

		char c = ConvertStart(grid[currentPoint.X, currentPoint.Y], currentPoint);

		CheckEnclosedPoints(c);
	}
}

DrawGrid(star1);

int star2 = enclosedPoints.Count - graph.VertexCount;

// Answer: 357
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

	if (point.X < grid.GetUpperBound(Dimension_X))
	{
		char target = grid[point.X + 1, point.Y];

		if (source is 'S' or '-' or 'L' or 'F' &&
			target is 'S' or '-' or 'J' or '7')
		{
			yield return Direction.East;
		}
	}

	if (point.Y < grid.GetUpperBound(Dimension_Y))
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

void DrawGrid(int maxDistance, Point? currentPoint = null)
{
	for (int y = 0; y <= grid.GetUpperBound(Dimension_Y); y++)
	{
		for (int x = 0; x <= grid.GetUpperBound(Dimension_X); x++)
		{
			Point point = new(x, y);
			int distance = distances[x, y];
			char c = grid[x, y];

			if (point == currentPoint)
			{
				Console.BackgroundColor = ConsoleColor.Yellow;
			}

			if (distance == maxDistance || c == 'S')
			{
				ConsoleEx.WriteRgb($"{ConvertChar(c)}", Color.Red);
				continue;
			}

			if (distance == -1)
			{
				if (enclosedPoints.Contains(new Point(x, y)))
				{
					ConsoleEx.WriteRgb($"{ConvertChar(c)}", Color.FromArgb(255, 0, 255));
				}
				else
				{
					Console.Write(ConvertChar(c));
				}
				continue;
			}

			int green = (int)(200d / maxDistance * distance) + 55;

			ConsoleEx.WriteRgb($"{ConvertChar(c)}", Color.FromArgb(0, green, 0));
		}

		Console.WriteLine();
	}
}

bool IsPointConnected(Point point)
{
	colors.TryGetValue($"{point.X},{point.Y}", out GraphColor verticeColor);

	bool isConnected = verticeColor == GraphColor.Black;

	return isConnected;
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

void CheckDirection(Direction direction, Point startPosition)
{
	if (direction.HasFlag(Direction.North) && direction.HasFlag(Direction.East))
	{
		for (int y = startPosition.Y - 1; y >= 0; y--)
		{
			for (int x = startPosition.X + 1; x <= grid.GetUpperBound(Dimension_X); x++)
			{
				Point point = new(x, y);

				if (IsPointConnected(point))
				{
					return;
				}

				enclosedPoints.Remove(point);
			}
		}
	}

	if (direction.HasFlag(Direction.East) && direction.HasFlag(Direction.South))
	{
		for (int x = startPosition.X + 1; x <= grid.GetUpperBound(Dimension_X); x++)
		{
			for (int y = startPosition.Y + 1; y <= grid.GetUpperBound(Dimension_Y); y++)
			{
				Point point = new(x, y);

				if (IsPointConnected(point))
				{
					return;
				}

				enclosedPoints.Remove(point);
			}
		}
	}

	if (direction.HasFlag(Direction.South) && direction.HasFlag(Direction.West))
	{
		for (int y = startPosition.Y + 1; y <= grid.GetUpperBound(Dimension_Y); y++)
		{
			for (int x = startPosition.X - 1; x >= 0; x--)
			{
				Point point = new(x, y);

				if (IsPointConnected(point))
				{
					return;
				}

				enclosedPoints.Remove(point);
			}
		}
	}

	if (direction.HasFlag(Direction.West) && direction.HasFlag(Direction.North))
	{
		for (int x = startPosition.X - 1; x >= 0; x--)
		{
			for (int y = startPosition.Y - 1; y >= 0; y--)
			{
				Point point = new(x, y);

				if (IsPointConnected(point))
				{
					return;
				}

				enclosedPoints.Remove(point);
			}
		}
	}

	if (direction is Direction.North)
	{
		for (int y = startPosition.Y - 1; y >= 0; y--)
		{
			Point point = new(startPosition.X, y);

			if (IsPointConnected(point))
			{
				return;
			}

			enclosedPoints.Remove(point);
		}
	}

	if (direction is Direction.East)
	{
		for (int x = startPosition.X + 1; x <= grid.GetUpperBound(Dimension_X); x++)
		{
			Point point = new(x, startPosition.Y);

			if (IsPointConnected(point))
			{
				return;
			}

			enclosedPoints.Remove(point);
		}
	}

	if (direction is Direction.South)
	{
		for (int y = startPosition.Y + 1; y <= grid.GetUpperBound(Dimension_Y); y++)
		{
			Point point = new(startPosition.X, y);

			if (IsPointConnected(point))
			{
				return;
			}

			enclosedPoints.Remove(point);
		}
	}

	if (direction is Direction.West)
	{
		for (int x = startPosition.X - 1; x >= 0; x--)
		{
			Point point = new(x, startPosition.Y);

			if (IsPointConnected(point))
			{
				return;
			}

			enclosedPoints.Remove(point);
		}
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

char ConvertStart(char c, Point point)
{
	if (c is 'S')
	{
		IEnumerable<Direction> startDirections = GetDirections(point);

		if (startDirections.Contains(Direction.East) && startDirections.Contains(Direction.South))
		{
			c = 'F';
		}

		if (startDirections.Contains(Direction.West) && startDirections.Contains(Direction.South))
		{
			c = '7';
		}

		if (startDirections.Contains(Direction.North) && startDirections.Contains(Direction.West))
		{
			c = 'J';
		}

		if (startDirections.Contains(Direction.North) && startDirections.Contains(Direction.East))
		{
			c = 'L';
		}
	}

	return c;
}

void CheckEnclosedPoints(char c, bool changeDirection = true)
{
	switch (c)
	{
		case 'F':
			// Check on west and north, until connected node
			if (currentDirection is Direction.North or Direction.East)
			{
				CheckDirection(Direction.North, currentPoint);
				CheckDirection(Direction.West, currentPoint);
				CheckDirection(Direction.North | Direction.West, currentPoint);
			}

			currentDirection = changeDirection ? (currentDirection is Direction.North ? Direction.East : Direction.South) : currentDirection;
			//Console.WriteLine($"Going to {currentDirection}");
			break;

		case '7':
			// Check north and east, until connected node
			if (currentDirection is Direction.East)
			{
				CheckDirection(Direction.North, currentPoint);
				CheckDirection(Direction.East, currentPoint);
				CheckDirection(Direction.North | Direction.East, currentPoint);
			}

			currentDirection = changeDirection ? (currentDirection is Direction.East ? Direction.South : Direction.West) : currentDirection;
			//Console.WriteLine($"Going to {currentDirection}");
			break;

		case 'J':
			// Check east and south, until connected node
			if (currentDirection is Direction.South)
			{
				CheckDirection(Direction.East, currentPoint);
				CheckDirection(Direction.South, currentPoint);
				CheckDirection(Direction.East | Direction.South, currentPoint);
			}

			currentDirection = changeDirection ? (currentDirection is Direction.South ? Direction.West : Direction.North) : currentDirection;
			//Console.WriteLine($"Going to {currentDirection}");
			break;

		case 'L':
			// Check south and west, until connected node
			if (currentDirection is Direction.West)
			{
				CheckDirection(Direction.South, currentPoint);
				CheckDirection(Direction.West, currentPoint);
				CheckDirection(Direction.South | Direction.West, currentPoint);
			}

			currentDirection = changeDirection ? (currentDirection is Direction.South ? Direction.East : Direction.North) : currentDirection;
			//Console.WriteLine($"Going to {currentDirection}");
			break;

		case '-':
			// currentDirection is East, check north, until connected node
			// currentDirection is West, check south, until connected node

			if (currentDirection is Direction.East)
			{
				CheckDirection(Direction.North, currentPoint);
			}

			if (currentDirection is Direction.West)
			{
				CheckDirection(Direction.South, currentPoint);
			}

			break;

		case '|':
			// currentDirection is south, check east, until connected node
			// currentDirection is north, check west, until connected node

			if (currentDirection is Direction.South)
			{
				CheckDirection(Direction.East, currentPoint);
			}

			if (currentDirection is Direction.North)
			{
				CheckDirection(Direction.West, currentPoint);
			}

			break;
	}
}

[Flags]
public enum Direction
{
	North = 1,
	East = 2,
	South = 4,
	West = 8
}
