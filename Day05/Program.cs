using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

partial class Program
{
    [GeneratedRegex("move (?<Count>\\d+) from (?<From>\\d+) to (?<To>\\d+)")]
    private static partial Regex CommandRegex();

    private static async Task Main(string[] args)
    {
        string title = "AdventOfCode2022 - Day 05";
        Console.Title = title;
        ConsoleEx.WriteLine(title, ConsoleColor.Green);

        List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

        Stopwatch stopwatch = Stopwatch.StartNew();

        List<Stack<char>> board1 = MoveCrates(inputLines, star1: true);

        // Answer: SPFMVDTZT
        ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {new string(board1.Select(x => x.Peek()).ToArray())}", ConsoleColor.Yellow);

        stopwatch.Restart();

        List<Stack<char>> board2 = MoveCrates(inputLines, star1: false);

        // Answer: ZFSJBPRFP
        ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {new string(board2.Select(x => x.Peek()).ToArray())}", ConsoleColor.Yellow);

        ConsoleEx.WriteLine("END", ConsoleColor.Green);
        Console.ReadKey();
    }

    private static List<Stack<char>> MoveCrates(List<string> inputLines, bool star1)
    {
        Regex commandRegex = CommandRegex();

        List<string> board = new();
        bool boardFound = false;
        List<Stack<char>> stacks = new(Enumerable.Range(0, (int)Math.Ceiling(inputLines.First().Length / 4d)).Select(x => new Stack<char>()));

        foreach (string input in inputLines)
        {
            if (!boardFound)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    boardFound = true;

                    for (int i = board.Count - 2; i >= 0; i--)
                    {
                        string boardLine = board[i];

                        for (int stack = 0; stack < stacks.Count; stack++)
                        {
                            char crate = boardLine[stack * 4 + 1];

                            if (crate != ' ')
                            {
                                stacks[stack].Push(crate);
                            }
                        }
                    }
                }
                else
                {
                    board.Add(input);
                }
            }
            else
            {
                Match commandRegexMatch = commandRegex.Match(input);
                int count = int.Parse(commandRegexMatch.Groups["Count"].Value);
                int from = int.Parse(commandRegexMatch.Groups["From"].Value) - 1;
                int to = int.Parse(commandRegexMatch.Groups["To"].Value) - 1;

                if (star1)
                {
                    for (int action = 0; action < count; action++)
                    {
                        stacks[to].Push(stacks[from].Pop());
                    }
                }
                else
                {
                    Stack<char> tempStack = new();

                    for (int action = 0; action < count; action++)
                    {
                        tempStack.Push(stacks[from].Pop());
                    }

                    while(tempStack.Any())
                    {
                        stacks[to].Push(tempStack.Pop());
                    }
                }
            }
        }

        return stacks;
    }
}