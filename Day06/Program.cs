using Library;
using System.Diagnostics;

string title = "AdventOfCode2022 - Day 06";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

int startOfPacketMarkerStar1 = GetStartOfPacketMarker(inputLines.First(), 4);

// Answer: 1848
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {startOfPacketMarkerStar1}", ConsoleColor.Yellow);

stopwatch.Restart();

int startOfPacketMarkerStar2 = GetStartOfPacketMarker(inputLines.First(), 14);

// Answer: 2308
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {startOfPacketMarkerStar2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

static int GetStartOfPacketMarker(string input, int distinctChars)
{
    Queue<char> chars = new(distinctChars);

    for (int i = 0; i < input.Length; i++)
    {
        chars.Enqueue(input[i]);

        if (chars.Count == distinctChars)
        {
            if (chars.Distinct().Count() == distinctChars)
            {
                return i + 1;
            }

            chars.Dequeue();
        }
    }

    return -1;
}
