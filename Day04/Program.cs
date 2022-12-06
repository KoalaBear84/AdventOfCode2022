using Library;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

string title = "AdventOfCode2022 - Day 04";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

int overlaps = 0;
int partialOverlaps = 0;

foreach (string input in inputLines)
{
    string[] inputSplitted = input.Split(',');
    int[] clean1 = inputSplitted[0].Split('-').Select(int.Parse).ToArray();
    int[] clean2 = inputSplitted[1].Split('-').Select(int.Parse).ToArray();

    if (
        (clean1[0] <= clean2[0] && clean1[1] >= clean2[1]) ||
        (clean2[0] <= clean1[0] && clean2[1] >= clean1[1])
        )
    {
        overlaps++;
    }

    if (clean1[1] >= clean2[0] &&
        clean2[1] >= clean1[0])
    {
        partialOverlaps++;
    }
}

// Answer: 490
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {overlaps}", ConsoleColor.Yellow);

stopwatch.Restart();

// Answer: 921
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {partialOverlaps}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();