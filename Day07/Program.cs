using Library;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

string title = "AdventOfCode2022 - Day 07";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

Regex commandRegex = CommandRegex();

string command = string.Empty;
FileSystemEntry root = new();
FileSystemEntry currentWorkingDirectory = root;

foreach (string input in inputLines)
{
    Match commandRegexMatch = commandRegex.Match(input);

    if (commandRegexMatch.Success)
    {
        bool isCommand = commandRegexMatch.Groups["IsCommand"].Success;
        string param1 = commandRegexMatch.Groups["Param1"].Value;
        string param2 = commandRegexMatch.Groups["Param2"].Value;

        if (isCommand)
        {
            command = param1;

            if (command == "cd")
            {
                if (param2 == "/")
                {
                    currentWorkingDirectory = root;
                }
                else if (param2 == ".." && currentWorkingDirectory.Parent is not null)
                {
                    currentWorkingDirectory = currentWorkingDirectory.Parent;
                }
                else
                {
                    FileSystemEntry fileSystemEntry = currentWorkingDirectory.Entries.FirstOrDefault(e => e.Name == param2);

                    if (fileSystemEntry is not null)
                    {
                        currentWorkingDirectory = fileSystemEntry;
                    }
                }
            }
            else if (command == "ls")
            {
                // Ignore
            }
        }
        else if (command == "ls")
        {
            FileSystemEntry fileSystemEntry = new()
            {
                Parent = currentWorkingDirectory,
                IsDirectory = param1 == "dir",
                Name = param2
            };

            if (param1 == "dir")
            {
                currentWorkingDirectory.Entries.Add(fileSystemEntry);
            }
            else
            {
                fileSystemEntry.FileSize = int.Parse(param1);

                currentWorkingDirectory.Entries.Add(fileSystemEntry);
            }
        }
    }
}

Dictionary<FileSystemEntry, int> sizes = new();

GetSumOfDirectoriesBelowSize(root, sizes);

// Answer:  1390824
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {sizes.Where(x => x.Value <= 100_000).Select(x => x.Value).Sum():n0}", ConsoleColor.Yellow);

stopwatch.Restart();

int fileSystemSize = 70_000_000;
int neededFreeSpace = 30_000_000;
int freeSpace = fileSystemSize - root.GetSize(recursive: true);

int lowest = sizes.Where(s => freeSpace + s.Value > neededFreeSpace).OrderBy(x => x.Value).Select(x => x.Value).ToList().First();

// Answer: 7490863
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {lowest}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

void GetSumOfDirectoriesBelowSize(FileSystemEntry root, Dictionary<FileSystemEntry, int> sizes)
{
    sizes.Add(root, root.GetSize(recursive: true));

    foreach (FileSystemEntry entry in root.Entries.Where(x => x.IsDirectory))
    {
        GetSumOfDirectoriesBelowSize(entry, sizes);
    }
}

partial class Program
{
    [GeneratedRegex("(?<IsCommand>\\$)? ?(?<Param1>\\S*) ?(?<Param2>\\S.*)?")]
    private static partial Regex CommandRegex();
}

public class FileSystemEntry
{
    public FileSystemEntry? Parent { get; set; }
    public bool IsDirectory { get; set; } = true;
    public string? Name { get; set; }
    public int FileSize { get; set; }

    public List<FileSystemEntry> Entries { get; set; } = new();

    public int GetSize(bool recursive = false)
    {
        if (!recursive)
        {
            return Entries.Sum(e => e.FileSize);
        }
        else
        {
            return Entries.Sum(e => e.FileSize) + Entries.Select(e => e.GetSize(recursive: recursive)).Sum();
        }
    }
}