using Library;
using System.Diagnostics;

string title = "AdventOfCode2023 - Day 07";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

List<HandStar1> handsStar1 = [];

foreach (string input in inputLines)
{
	string[] splitted = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

	handsStar1.Add(new HandStar1
	{
		Cards = splitted[0][..5],
		Bid = int.Parse(splitted[1])
	});
}

//- Five of a kind, where all five cards have the same label: AAAAA
//Console.WriteLine(new HandStar1 { Cards = "AAAAA" }.HandType);
//- Four of a kind, where four cards have the same label and one card has a different label: AA8AA
//Console.WriteLine(new HandStar1 { Cards = "AA8AA" }.HandType);
//- Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
//Console.WriteLine(new HandStar1 { Cards = "23332" }.HandType);
//- Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
//Console.WriteLine(new HandStar1 { Cards = "TTT98" }.HandType);
//- Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
//Console.WriteLine(new HandStar1 { Cards = "23432" }.HandType);
//- One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
//Console.WriteLine(new HandStar1 { Cards = "A23A4" }.HandType);
//- High card, where all cards' labels are distinct: 23456
//Console.WriteLine(new HandStar1 { Cards = "23456" }.HandType);

IOrderedEnumerable<Hand> orderedHandsStar1 = handsStar1.Order(new HandComparerStar1());

foreach (HandStar1 hand in orderedHandsStar1)
{
	//Console.WriteLine($"{new string(hand.Cards)}\t{hand.Bid}\t{hand.HandType}");
}

int star1 = orderedHandsStar1.Select((hand, index) =>
{
	int rank = handsStar1.Count - index;
	int score = rank * hand.Bid;
	//Console.WriteLine($"{new string(hand.Cards)}\t{hand.Bid}\t{hand.HandType}\t{rank} * {hand.Bid} = {score}");
	return score;
}).Sum();

// Answer: 249483956
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

List<HandStar2> handsStar2 = [];

foreach (string input in inputLines)
{
	string[] splitted = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

	handsStar2.Add(new HandStar2
	{
		Cards = splitted[0][..5],
		Bid = int.Parse(splitted[1])
	});
}

IOrderedEnumerable<Hand> orderedHandStar2 = handsStar2.Order(new HandComparerStar2());

foreach (HandStar2 hand in orderedHandStar2)
{
	//Console.WriteLine($"{new string(hand.Cards)}\t{hand.Bid}\t{hand.HandType}");
}

int star2 = orderedHandStar2.Select((hand, index) =>
{
	int rank = handsStar2.Count - index;
	int score = rank * hand.Bid;
	//Console.WriteLine($"{new string(hand.Cards)}\t{hand.Bid}\t{hand.HandType}\t{rank} * {hand.Bid} = {score}");
	return score;
}).Sum();

// Answer: 252137472
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

public abstract class Hand
{
	public string Cards { get; set; }
	public int Bid { get; set; }
	public HandType HandType;
}

public class HandStar1 : Hand
{
	public new HandType HandType => GetHandType(this);

	public static HandType GetHandType(Hand hand)
	{
		List<IGrouping<char, char>> grouped = [.. hand.Cards.Select(x => x).GroupBy(x => x).OrderByDescending(x => x.Count())];

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

public class HandComparerStar1 : IComparer<HandStar1>
{
	private readonly List<char> Score = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];

	public int Compare(HandStar1? x, HandStar1? y)
	{
		if (x.HandType > y.HandType)
		{
			return -1;
		}

		if (y.HandType > x.HandType)
		{
			return 1;
		}

		for (int i = 0; i < x.Cards.Length; i++)
		{
			char cardSelf = x.Cards[i];
			char cardOther = y.Cards[i];

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
}

public class HandStar2 : Hand
{
	public new HandType HandType => GetHandType(this);

	public static HandType GetHandType(Hand hand)
	{
		List<IGrouping<char, char>> grouped = [.. hand.Cards.Where(x => x != 'J') .GroupBy(x => x).OrderByDescending(x => x.Count())];

		int sameCards = grouped.Count != 0 ? grouped[0]?.Count() ?? 0 : 0;
		int jacks = hand.Cards.Where(x => x == 'J').Count();

		if (sameCards + jacks == 5)
		{
			return HandType.FiveOfAKind;
		}

		if (sameCards + jacks == 4)
		{
			return HandType.FourOfAKind;
		}

		if (sameCards + jacks == 3)
		{
			if (grouped[1].Count() == 2)
			{
				return HandType.FullHouse;
			}

			return HandType.ThreeOfAKind;
		}

		if (sameCards + jacks == 2)
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

public class HandComparerStar2 : IComparer<HandStar2>
{
	private readonly List<char> Score = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

	public int Compare(HandStar2? x, HandStar2? y)
	{
		if (x.HandType > y.HandType)
		{
			return -1;
		}

		if (y.HandType > x.HandType)
		{
			return 1;
		}

		for (int i = 0; i < x.Cards.Length; i++)
		{
			char cardSelf = x.Cards[i];
			char cardOther = y.Cards[i];

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
}