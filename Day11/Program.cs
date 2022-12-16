using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

string title = "AdventOfCode2022 - Day 11";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

string input = (await File.ReadAllTextAsync("input.txt"));

Regex monkeyRegex = MonkeyRegex();

Stopwatch stopwatch = Stopwatch.StartNew();

List<Match> monkeyRegexMatches = monkeyRegex.Matches(input).Cast<Match>().ToList();

List<Monkey> monkeys = ParseMonkeys();

MonkeyBusiness(20, 3);

foreach (Monkey monkey in monkeys)
{
    Console.WriteLine($"Monkey {monkey.Number} ({monkey.InspectedItems}): {string.Join(", ", monkey.Items)}");
}

IEnumerable<ulong> mostActiveMonkeys = monkeys.Select(x => x.InspectedItems).OrderDescending().Take(2);
// Answer: 56120
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {mostActiveMonkeys.First() * mostActiveMonkeys.Last()}", ConsoleColor.Yellow);

stopwatch.Restart();

monkeys = ParseMonkeys();

MonkeyBusiness(10_000, 1);

foreach (Monkey monkey in monkeys)
{
    Console.WriteLine($"Monkey {monkey.Number} ({monkey.InspectedItems}): {string.Join(", ", monkey.Items)}");
}

mostActiveMonkeys = monkeys.Select(x => x.InspectedItems).OrderDescending().Take(2);

// Answer: 24389045529
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {mostActiveMonkeys.First() * mostActiveMonkeys.Last()}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

List<Monkey> ParseMonkeys()
{
    List<Monkey> monkeys = new();

    foreach (Match monkeyRegexMatch in monkeyRegexMatches)
    {
        if (monkeyRegexMatch.Success)
        {
            Monkey monkey = new()
            {
                Number = int.Parse(monkeyRegexMatch.Groups["Number"].Value),
                Items = monkeyRegexMatch.Groups["Items"].Value.Split(',').Select(x => ulong.Parse(x.Trim())).ToList(),
                DivisibleBy = ulong.Parse(monkeyRegexMatch.Groups["DivisibleBy"].Value),
                OperationOperator = monkeyRegexMatch.Groups["OperationOperator"].Value,
                OperationParameter = monkeyRegexMatch.Groups["OperationParameter"].Value,
                ThrowToMonkeyIfTrue = int.Parse(monkeyRegexMatch.Groups["ThrowToMonkeyIfTrue"].Value),
                ThrowToMonkeyIfFalse = int.Parse(monkeyRegexMatch.Groups["ThrowToMonkeyIfFalse"].Value)
            };

            monkeys.Add(monkey);
        }
    }

    return monkeys;
}

void MonkeyBusiness(int rounds, ulong divideBy)
{
    ulong divisbleByModulo = monkeys.Select(x => x.DivisibleBy).Aggregate((a, b) => a * b);

    for (int i = 0; i < rounds; i++)
    {
        foreach (Monkey monkey in monkeys)
        {
            foreach (ulong item in monkey.Items.ToList())
            {
                monkey.InspectedItems++;

                ulong itemLocal = item;
                ulong operatorInput = 0;

                if (monkey.OperationParameter == "old")
                {
                    operatorInput = itemLocal;
                }
                else
                {
                    operatorInput = ulong.Parse(monkey.OperationParameter);
                }

                if (monkey.OperationOperator == "*")
                {
                    itemLocal *= operatorInput;
                }
                else if (monkey.OperationOperator == "+")
                {
                    itemLocal += operatorInput;
                }

                itemLocal /= divideBy;
                itemLocal %= divisbleByModulo;

                int throwToMonkey = itemLocal % monkey.DivisibleBy == 0 ? monkey.ThrowToMonkeyIfTrue : monkey.ThrowToMonkeyIfFalse;
                monkeys[throwToMonkey].Items.Add(itemLocal);

                monkey.Items.Remove(item);
            }
        }
    }
}

[DebuggerDisplay("{Number} {Items}")]
public class Monkey
{
    public int Number { get; set; }
    public List<ulong> Items { get; set; } = new();
    public ulong InspectedItems { get; set; }
    public string? OperationOperator { get; set; }
    public string? OperationParameter { get; set; }
    public ulong DivisibleBy { get; set; }
    public int ThrowToMonkeyIfTrue { get; set; }
    public int ThrowToMonkeyIfFalse { get; set; }
}

partial class Program
{
    [GeneratedRegex("Monkey (?<Number>\\d+):\\s+Starting items: (?<Items>(?:\\d+,?\\s*)+)Operation: new = old (?<OperationOperator>\\S*) (?<OperationParameter>\\S*)\\s*Test: divisible by (?<DivisibleBy>\\d+)\\s+If true: throw to monkey (?<ThrowToMonkeyIfTrue>\\d+)\\s+If false: throw to monkey (?<ThrowToMonkeyIfFalse>\\d+)")]
    private static partial Regex MonkeyRegex();
}