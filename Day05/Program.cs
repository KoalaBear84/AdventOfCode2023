using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

string title = "AdventOfCode2023 - Day 04";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = [.. (await File.ReadAllLinesAsync("input.txt"))];

Stopwatch stopwatch = Stopwatch.StartNew();

List<Mapping> SeedToSoil = [];
List<Mapping> SoilToFertilizer = [];
List<Mapping> FertilizerToWater = [];
List<Mapping> WaterToLight = [];
List<Mapping> LightToTemperature = [];
List<Mapping> TemperatureToHumidity = [];
List<Mapping> HumidityToLocation = [];

List<Mapping> CurrentMap = null;

List<long> Seeds = [];

Regex mapRegex = MapRegex();

foreach (string input in inputLines)
{
	if (input.StartsWith("seeds:"))
	{
		Seeds.AddRange(input.Substring(7).Split(' ').Select(long.Parse));
	}
	else if (input.EndsWith(':'))
	{
		CurrentMap = input switch
		{
			"seed-to-soil map:" => SeedToSoil,
			"soil-to-fertilizer map:" => SoilToFertilizer,
			"fertilizer-to-water map:" => FertilizerToWater,
			"water-to-light map:" => WaterToLight,
			"light-to-temperature map:" => LightToTemperature,
			"temperature-to-humidity map:" => TemperatureToHumidity,
			"humidity-to-location map:" => HumidityToLocation,
			_ => throw new NotImplementedException()
		};
	}
	else
	{
		Match regexMatch = mapRegex.Match(input);

		if (regexMatch.Success)
		{
			CurrentMap.Add(new Mapping
			{
				DestinationRangeStart = long.Parse(regexMatch.Groups["DestinationRangeStart"].Value),
				SourceRangeStart = long.Parse(regexMatch.Groups["SourceRangeStart"].Value),
				RangeLength = long.Parse(regexMatch.Groups["RangeLength"].Value)
			});
		}
	}
}

long star1 = long.MaxValue;

foreach (long seed in Seeds)
{
	long soil = GetTargetValue(SeedToSoil, seed);
	long fertilizer = GetTargetValue(SoilToFertilizer, soil);
	long water = GetTargetValue(FertilizerToWater, fertilizer);
	long light = GetTargetValue(WaterToLight, water);
	long temperature = GetTargetValue(LightToTemperature, light);
	long humidity = GetTargetValue(TemperatureToHumidity, temperature);
	long location = GetTargetValue(HumidityToLocation, humidity);

	star1 = Math.Min(star1, location);
}

// Answer: 157211394
ConsoleEx.WriteLine($"Star 1. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star1}", ConsoleColor.Yellow);

stopwatch.Restart();

int star2 = -1;

// Answer: 
ConsoleEx.WriteLine($"Star 2. {TimerHelper.GetMilliseconds(stopwatch):n2}ms. Answer: {star2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

static long GetTargetValue(List<Mapping>? mappings, long input)
{
	foreach (Mapping mapping in mappings)
	{
		if (input >= mapping.SourceRangeStart && input <= mapping.SourceRangeStart + mapping.RangeLength)
		{
			return mapping.DestinationRangeStart + input - mapping.SourceRangeStart;
		}
	}

	return input;
}

public class Mapping
{
	public long DestinationRangeStart { get; set; }
	public long SourceRangeStart { get; set; }
	public long RangeLength { get; set; }
}

partial class Program
{
	[GeneratedRegex("(?<DestinationRangeStart>\\d+)\\W+(?<SourceRangeStart>\\d+)\\W+(?<RangeLength>\\d+)")]
	private static partial Regex MapRegex();
}