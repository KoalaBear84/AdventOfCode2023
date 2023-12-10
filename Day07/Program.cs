using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day 07";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

List<Hand> hands = [];

foreach (string input in inputLines)
{
	string[] splitted = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

	hands.Add(new Hand
	{
		Cards = splitted[0].Take(5).ToArray(),
		Bid = int.Parse(splitted[1])
	});
}

//- Five of a kind, where all five cards have the same label: AAAAA
//Console.WriteLine(GetHandType(new Hand { Cards = ['A', 'A', 'A', 'A', 'A'] }));
//- Four of a kind, where four cards have the same label and one card has a different label: AA8AA
//Console.WriteLine(GetHandType(new Hand { Cards = ['A', 'A', '8', 'A', 'A'] }));
//- Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
//Console.WriteLine(GetHandType(new Hand { Cards = ['2', '3', '3', '3', '2'] }));
//- Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
//Console.WriteLine(GetHandType(new Hand { Cards = ['T', 'T', 'T', '9', '8'] }));
//- Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
//Console.WriteLine(GetHandType(new Hand { Cards = ['2', '3', '4', '3', '2'] }));
//- One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
//Console.WriteLine(GetHandType(new Hand { Cards = ['A', '2', '3', 'A', '4'] }));
//- High card, where all cards' labels are distinct: 23456
//Console.WriteLine(GetHandType(new Hand { Cards = ['2', '3', '4', '5', '6'] }));

IOrderedEnumerable<Hand> orderedHands = hands.Order();

foreach (Hand hand in orderedHands)
{
	//Console.WriteLine($"{new string(hand.Cards)}\t{hand.Bid}\t{hand.HandType}");
}

int star1 = orderedHands.Select((hand, index) =>
{
	int rank = hands.Count - index;
	int score = rank * hand.Bid;
	//Console.WriteLine($"{new string(hand.Cards)}\t{hand.Bid}\t{hand.HandType}\t{rank} * {hand.Bid} = {score}");
	return score;
}).Sum();

// Answer: 249483956
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = -1;

// Answer: 
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

public enum HandType
{
	FiveOfAKind = 7,
	FourOfAKind = 6,
	FullHouse = 5,
	ThreeOfAKind = 4,
	TwoPair = 3,
	OnePair = 2,
	HighCard = 1,
	Unknown = 0
}

public class Hand : IComparable<Hand>
{
	public char[] Cards { get; set; } = new char[5];
	public int Bid { get; set; }
	public HandType HandType => GetHandType(this);
	public static List<char> Score = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];

	public int CompareTo(Hand? other)
	{
		if (HandType > other.HandType)
		{
			return -1;
		}

		if (other.HandType > HandType)
		{
			return 1;
		}

		for (int i = 0; i < Cards.Length; i++)
		{
			char cardSelf = Cards[i];
			char cardOther = other.Cards[i];

			if (Score.IndexOf(cardSelf) < Score.IndexOf(cardOther))
			{
				return -1;
			}

			if (Score.IndexOf(cardSelf) > Score.IndexOf(cardOther))
			{
				return 1;
			}
		}

		return 0;
	}

	public static HandType GetHandType(Hand hand)
	{
		List<IGrouping<char, char>> grouped = hand.Cards.GroupBy(x => x).OrderByDescending(x => x.Count()).ToList();

		int sameCards = grouped[0].Count();

		if (sameCards == 5)
		{
			return HandType.FiveOfAKind;
		}

		if (sameCards == 4)
		{
			return HandType.FourOfAKind;
		}

		if (sameCards == 3)
		{
			if (grouped[1].Count() == 2)
			{
				return HandType.FullHouse;
			}

			return HandType.ThreeOfAKind;
		}

		if (sameCards == 2)
		{
			if (grouped[1].Count() == 2)
			{
				return HandType.TwoPair;
			}

			return HandType.OnePair;
		}

		if (grouped.Count == 5)
		{
			return HandType.HighCard;
		}

		return HandType.Unknown;
	}
}