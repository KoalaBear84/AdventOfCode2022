using Library;
using System.Data.Common;
using System.Diagnostics;

string title = "AdventOfCode2022 - Day 08";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

int rows = inputLines.Count;
int columns = inputLines.First().Length;

int[][] map = new int[rows][];

for (int row = 0; row < inputLines.Count; row++)
{
    string input = inputLines[row];

    map[row] = new int[columns];

    map[row] = input.ToArray().Select(x => int.Parse(x.ToString())).ToArray();
}

int visibleTrees = 0;

Console.WriteLine();

for (int row = 0; row < rows; row++)
{
    for (int col = 0; col < columns; col++)
    {
        int tempVisibleCount = VisibleCount(map, row, col);

        if (tempVisibleCount > 0)
        {
            visibleTrees += tempVisibleCount;
        }

        //Console.ForegroundColor = tempVisibleCount > 0 ? ConsoleColor.Green : ConsoleColor.Gray;
        //Console.Write(map[row][col].ToString());
    }

    //Console.WriteLine();
}

//Console.WriteLine();

int VisibleCount(int[][] map, int y, int x)
{
    // Edges
    if (x == 0 || y == 0 || x == map.Length - 1 || y == map[0].Length - 1)
    {
        return 1;
    }

    int checkTree = map[y][x];

    int? highestTree;

    highestTree = null;

    for (int row = 0; row <= y; row++)
    {
        int currentTree = map[row][x];

        if (highestTree.HasValue)
        {
            if (highestTree >= checkTree)
            {
                break;
            }
        }

        if (row == y)
        {
            return 1;
        }

        highestTree = Math.Max(currentTree, highestTree ?? 0);
    }

    highestTree = null;

    for (int row = map[x].Length - 1; row >= y; row--)
    {
        int currentTree = map[row][x];

        if (highestTree.HasValue)
        {
            if (highestTree >= checkTree)
            {
                break;
            }
        }

        if (row == y)
        {
            return 1;
        }

        highestTree = Math.Max(currentTree, highestTree ?? 0);
    }

    highestTree = null;

    for (int column = 0; column <= x; column++)
    {
        int currentTree = map[y][column];

        if (highestTree.HasValue)
        {
            if (highestTree >= checkTree)
            {
                break;
            }
        }

        if (column == x)
        {
            return 1;
        }

        highestTree = Math.Max(currentTree, highestTree ?? 0);
    }

    highestTree = null;

    for (int column = map[y].Length - 1; column >= x; column--)
    {
        int currentTree = map[y][column];

        if (highestTree.HasValue)
        {
            if (highestTree >= checkTree)
            {
                break;
            }
        }

        if (column == x)
        {
            return 1;
        }

        highestTree = Math.Max(currentTree, highestTree ?? 0);
    }

    return 0;
}

// Answer: 1690
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {visibleTrees}", ConsoleColor.Yellow);

stopwatch.Restart();

int ScenicScore(int[][] map, int y, int x)
{
    int checkTree = map[y][x];

    int top = 0;
    int right = 0;
    int bottom = 0;
    int left = 0;

    // To top
    for (int row = y - 1; row >= 0; row--)
    {
        int currentTree = map[row][x];

        top++;

        if (currentTree >= checkTree)
        {
            break;
        }
    }

    // To bottom
    for (int row = y + 1; row < map[y].Length; row++)
    {
        int currentTree = map[row][x];

        bottom++;

        if (currentTree >= checkTree)
        {
            break;
        }
    }

    // To left
    for (int column = x - 1; column >= 0; column--)
    {
        int currentTree = map[y][column];

        left++;

        if (currentTree >= checkTree)
        {
            break;
        }
    }

    // To right
    for (int column = x + 1; column < map[x].Length; column++)
    {
        int currentTree = map[y][column];

        right++;

        if (currentTree >= checkTree)
        {
            break;
        }
    }

    return top * right * bottom * left;
}

int highestScenicScore = 0;

for (int row = 0; row < rows; row++)
{
    for (int col = 0; col < columns; col++)
    {
        int scenicScore = ScenicScore(map, row, col);

        highestScenicScore = Math.Max(scenicScore, highestScenicScore);
    }
}

// Answer: 
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {highestScenicScore}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();