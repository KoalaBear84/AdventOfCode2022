using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

string title = "AdventOfCode2022 - Day 10";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

Regex commandRegex = CommandRegex();

int cycle = 1;
int register = 1;
int signalStrength = 0;
int crtRow = 0;

bool[][] crt = new bool[6][];

for (int i = 0; i < 6; i++)
{
    crt[i] = new bool[40];
}

foreach (string input in inputLines)
{
    Match commandRegexMatch = commandRegex.Match(input);

    if (commandRegexMatch.Success)
    {
        string command = commandRegexMatch.Groups["Command"].Value;

        if (command == "noop")
        {
            DoCycle();

        }
        else if (command == "addx")
        {
            DoCycle();

            int param = int.Parse(commandRegexMatch.Groups["Parameter"].Value);

            register += param;

            DoCycle();
        }
    }
}

void DoCycle()
{
    int crtPosition = cycle % 40;

    if (Math.Abs(crtPosition - register) <= 1)
    {
        crt[crtRow][crtPosition] = true;
    }

    cycle++;
    CalculateSignalStrength();
}

void CalculateSignalStrength()
{
    if (cycle == 20 || (cycle + 20) % 40 == 0)
    {
        int signalStrengthTemp = cycle * register;
        signalStrength += signalStrengthTemp;
    }

    if (cycle % 40 == 0)
    {
        crtRow++;
    }
}

// Answer: 12840
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {signalStrength}", ConsoleColor.Yellow);

stopwatch.Restart();

foreach (bool[] bools in crt)
{
    foreach (bool s in bools)
    {
        Console.Write(s ? "#" : ".");
    }

    Console.WriteLine();
}

// Answer: ZKJFBJFZ

/*
.###.#..#...##.####.###....##.####.####.
...#.#.#.....#.#....#..#....#.#.......#.
..#..##......#.###..###.....#.###....#..
.#...#.#.....#.#....#..#....#.#.....#...
#....#.#..#..#.#....#..#.#..#.#....#....
####.#..#..##..#....###...##..#....####.
*/

ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: ZKJFBJFZ", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

partial class Program
{
    [GeneratedRegex("(?<Command>\\S+)\\s?(?<Parameter>-?\\d+)?")]
    private static partial Regex CommandRegex();
}