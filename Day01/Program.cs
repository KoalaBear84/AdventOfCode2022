using Library;
using System.Diagnostics;

string title = "AdventOfCode202 - Day 01";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

List<int> caloriesPerElf = new();

int caloriesCurrentElf = 0;

foreach (string input in inputLines)
{
    if (string.IsNullOrEmpty(input))
    {
        caloriesPerElf.Add(caloriesCurrentElf);
        caloriesCurrentElf = 0;
        continue;
    }

    caloriesCurrentElf += int.Parse(input);
}

// Answer: 69206
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {caloriesPerElf.Max()}", ConsoleColor.Yellow);

stopwatch.Restart();

// Answer: 197400
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {caloriesPerElf.OrderDescending().Take(3).Sum()}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();