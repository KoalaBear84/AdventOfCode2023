using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

string title = "AdventOfCode2023 - Day 04";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

List<Card> allCards = [];

Regex cardRegex = CardRegex();

foreach (string input in inputLines)
{
	Match regexMatch = cardRegex.Match(input);

	if (regexMatch.Success)
	{
		allCards.Add(new Card
		{
			CardNumber = int.Parse(regexMatch.Groups["CardNumber"].Value),
			WinningNumbers = regexMatch.Groups["WinningNumbers"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse),
			MyNumbers = regexMatch.Groups["MyNumbers"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
		});
	}
}

Dictionary<int, int> cardPoints = allCards.ToDictionary((card) => card.CardNumber, card => GetPoints(GetMatches(card)));

foreach (KeyValuePair<int, int> cardPoint in cardPoints)
{
	//ConsoleEx.WriteLine($"Card {cardPoint.Key}: {cardPoint.Value}", ConsoleColor.Cyan);
}

int star1 = cardPoints.Sum(x => x.Value);

// Answer: 
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

Dictionary<int, int> cardCount = [];

int processedCards = 0;
int processedCardsWithMatches = 0;
int totalMatches = 0;
int totalPoints = 0;

// No, somehow I cannot understand my own logging, but it works..
void ProcessStar2(int sourceIndex, int startIndex, int count, int level = 0)
{
	//ConsoleEx.WriteLine($"{new string('\t', level)}Process cards from {sourceIndex + 1} -> {startIndex + 1} - {startIndex + count}", ConsoleColor.Yellow);

	for (int cardIndex = startIndex; cardIndex < startIndex + count; cardIndex++)
	{
		processedCards++;
		Card card = allCards[cardIndex];
		//ConsoleEx.WriteLine($"{new string('\t', level)}> Process card {card.CardNumber}", ConsoleColor.Cyan);

		int matches = GetMatches(card);

		totalMatches += matches;

		if (matches == 0)
		{
			//ConsoleEx.WriteLine($"{new string('\t', level)}>> no matches", ConsoleColor.Red);
			continue;
		}

		totalPoints += GetPoints(matches);
		processedCardsWithMatches++;

		//ConsoleEx.WriteLine($"{new string('\t', level)}>> {matches} matches", ConsoleColor.Green);

		cardCount.TryAdd(card.CardNumber, 0);
		cardCount[card.CardNumber]++;

		ProcessStar2(cardIndex, cardIndex + 1, matches, ++level);
	}
}

ProcessStar2(0, 0, allCards.Count);

int star2 = processedCards;

Console.WriteLine($"Processed Cards: {processedCards}");
Console.WriteLine($"Processed Cards With Matches: {processedCardsWithMatches}");
Console.WriteLine($"Total Matches: {totalMatches}");
Console.WriteLine($"Total Points: {totalPoints}");

// Answer: 5037841
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

static int GetMatches(Card card)
{
	return card.WinningNumbers.Intersect(card.MyNumbers).Count();
}

static int GetPoints(int matches)
{
	if (matches == 0)
	{
		return 0;
	}

	return (int)Math.Pow(2, matches - 1);
}

partial class Program
{
	[GeneratedRegex("Card\\W+(?<CardNumber>\\d+):\\W+(?<WinningNumbers>(?:\\d+\\W*?)+)\\|\\W+(?<MyNumbers>(?:\\d+\\W*)+)")]
	private static partial Regex CardRegex();
}

public class Card
{
	public int CardNumber { get; set; }
	public IEnumerable<int> WinningNumbers { get; set; } = [];
	public IEnumerable<int> MyNumbers { get; set; } = [];
}