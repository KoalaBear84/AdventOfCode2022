using Library;
using System.Diagnostics;

string title = "AdventOfCode2022 - Day 03";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

int sum = 0;
int sumBadges = 0;
int currentGroup = 0;
char[][] currentGroupInput = new char[3][];

foreach (string input in inputLines)
{
    int length = input.Length;
    char[] inputArray = input.ToArray();
    char[] compartment1 = inputArray[..(length / 2)];
    char[] compartment2 = inputArray[(length / 2)..];

    char sameSupply = compartment1.Intersect(compartment2).First();

    sum += Priority(sameSupply);

    currentGroupInput[currentGroup] = inputArray;

    if (currentGroup == 2)
    {
        currentGroup = 0;

        char badge = currentGroupInput.Skip(1).Aggregate(new HashSet<char>(currentGroupInput.First()), (h, e) => { h.IntersectWith(e); return h; }).First();
        sumBadges += Priority(badge);
    }
    else
    {
        currentGroup++;
    }
}

// Answer: 7845
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {sum}", ConsoleColor.Yellow);

// Answer: 2790
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {sumBadges}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

static int Priority(char input)
{
    if (!char.IsUpper(input))
    {
        return input - 'a' + 1;
    }
    else
    {
        return input - 'A' + 1 + 26;
    }
}