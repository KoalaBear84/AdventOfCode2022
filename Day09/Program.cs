using Library;
using System.Diagnostics;
using System.Drawing;

string title = "AdventOfCode2022 - Day 09";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

var tailPositionsStar1 = GetTailPositions(2);

// Answer: 6745
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {tailPositionsStar1.Select(k => new Point(k.X, k.Y)).ToHashSet().Count}", ConsoleColor.Yellow);

stopwatch.Restart();

var tailPositionsStar2 = GetTailPositions(10);

// Answer: 2793
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {tailPositionsStar2.Select(k => new Point(k.X, k.Y)).ToHashSet().Count}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();

List<Coordinate> GetTailPositions(int ropeLength)
{
    Coordinate[] knots = new Coordinate[ropeLength];

    // Fill rope
    for (int n = 0; n < knots.Length; n++)
    {
        knots[n] = new(0, 0);
    }

    List<Coordinate> TailPositions = new()
    {
        new Coordinate
        {
            X = 0,
            Y = 0
        }
    };

    foreach (string input in inputLines)
    {
        string[] splitted = input.Split(' ');
        char direction = splitted[0][0];
        int count = int.Parse(splitted[1]);

        int xShift = direction switch
        {
            'L' => -1,
            'R' => 1,
            _ => 0
        };

        int yShift = direction switch
        {
            'U' => -1,
            'D' => 1,
            _ => 0
        };

        for (int i = 0; i < count; i++)
        {
            knots[0].X += xShift;
            knots[0].Y += yShift;

            for (int j = 0; j < knots.Length - 1; j++)
            {
                Coordinate first = knots[j];
                Coordinate second = knots[j + 1];

                // This is all still ugly code
                int top = 0;
                int right = 0;
                int bottom = 0;
                int left = 0;

                int diffRows = first.Y - second.Y;
                int diffCols = first.X - second.X;

                bool muchLeft = diffCols < -1;
                bool muchRight = diffCols > 1;

                bool muchTop = diffRows < -1;
                bool muchBottom = diffRows > 1;

                bool toLeft = diffCols < 0;
                bool toRight = diffCols > 0;

                bool toTop = diffRows < 0;
                bool toBottom = diffRows > 0;

                if (muchLeft || ((muchTop || muchBottom) && toLeft))
                {
                    left = 1;
                }

                if (muchRight || ((muchTop || muchBottom) && toRight))
                {
                    right = 1;
                }

                if (muchTop || ((muchLeft || muchRight) && toTop))
                {
                    top = 1;
                }

                if (muchBottom || ((muchLeft || muchRight) && toBottom))
                {
                    bottom = 1;
                }

                int xShiftTemp = left * -1 + right * 1;
                int yShiftTemp = top * -1 + bottom * 1;

                if (xShiftTemp != 0 || yShiftTemp != 0)
                {
                    second.X += xShiftTemp;
                    second.Y += yShiftTemp;
                }
            }

            TailPositions.Add(new Coordinate
            {
                X = knots[^1].X,
                Y = knots[^1].Y
            });
        }
    }

    return TailPositions;
}

[DebuggerDisplay("{X}, {Y}")]
public class Coordinate
{
    public Coordinate() { }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X;
    public int Y;
}