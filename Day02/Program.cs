using Library;
using System.Diagnostics;

string title = "AdventOfCode2022 - Day 02";
Console.Title = title;
ConsoleEx.WriteLine(title, ConsoleColor.Green);

List<string> inputLines = (await File.ReadAllLinesAsync("input.txt")).ToList();

Stopwatch stopwatch = Stopwatch.StartNew();

int scoreStar1 = ProcessInput(inputLines, star1: true);

// Answer: 14297
ConsoleEx.WriteLine($"Star 1. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {scoreStar1}", ConsoleColor.Yellow);

stopwatch.Restart();

int scoreStar2 = ProcessInput(inputLines, star1: false);

// Answer: 16354 (Too high)
ConsoleEx.WriteLine($"Star 2. {stopwatch.Elapsed.Microseconds / 1000d:n2}ms. Answer: {scoreStar2}", ConsoleColor.Yellow);

ConsoleEx.WriteLine("END", ConsoleColor.Green);
Console.ReadKey();


static Result GameResult(ActionRPS actionRPS, ResponseRPS responseRPS)
{
    if ((int)actionRPS == (int)responseRPS)
    {
        return Result.Draw;
    }

    bool won =
        ((int)actionRPS == (int)RPS.Rock && (int)responseRPS == (int)RPS.Paper) ||
        ((int)actionRPS == (int)RPS.Paper && (int)responseRPS == (int)RPS.Scissors) ||
        ((int)actionRPS == (int)RPS.Scissors && (int)responseRPS == (int)RPS.Rock);

    return won ? Result.Win : Result.Lose;
}

static int Score(ResponseRPS responseRPS, Result result)
{
    int score = 0;

    if (result == Result.Win)
    {
        score += 6;
    }
    else if (result == Result.Draw)
    {
        score += 3;
    }

    score += (int)responseRPS;

    return score;
}

static int ProcessInput(List<string> inputLines, bool star1)
{
    int totalScore = 0;

    foreach (string input in inputLines)
    {
        string[] splitted = input.Split(' ');
        string action = splitted[0];
        string response = splitted[1];

        ActionRPS actionRPS = Enum.Parse<ActionRPS>(action);
        ResponseRPS responseRPS = Enum.Parse<ResponseRPS>(response);
        WantedResult wantedResult = Enum.Parse<WantedResult>(response);

        Result result = GameResult(actionRPS, responseRPS);

        if (star1)
        {
            totalScore += Score(responseRPS, result);
        }
        else
        {
            if ((int)wantedResult == (int)result)
            {
                totalScore += Score(responseRPS, result);
            }
            else
            {
                switch ((int)wantedResult)
                {
                    case (int)Result.Draw:
                        {
                            responseRPS = (ResponseRPS)actionRPS;
                            break;
                        }
                    case (int)Result.Lose:
                        {
                            if ((int)actionRPS == (int)RPS.Rock)
                            {
                                responseRPS = (ResponseRPS)RPS.Scissors;
                            }
                            else if ((int)actionRPS == (int)RPS.Paper)
                            {
                                responseRPS = (ResponseRPS)RPS.Rock;
                            }
                            else
                            {
                                responseRPS = (ResponseRPS)RPS.Paper;
                            }
                            break;
                        }
                    case (int)Result.Win:
                        {
                            if ((int)actionRPS == (int)RPS.Rock)
                            {
                                responseRPS = (ResponseRPS)RPS.Paper;
                            }
                            else if ((int)actionRPS == (int)RPS.Paper)
                            {
                                responseRPS = (ResponseRPS)RPS.Scissors;
                            }
                            else
                            {
                                responseRPS = (ResponseRPS)RPS.Rock;
                            }
                            break;
                        }
                }

                result = GameResult(actionRPS, responseRPS);

                totalScore += Score(responseRPS, result);
            }
        }
    }

    return totalScore;
}

enum Result
{
    Win,
    Lose,
    Draw
}

enum WantedResult
{
    X = Result.Lose,
    Y = Result.Draw,
    Z = Result.Win
}

enum RPS
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

enum ActionRPS
{
    A = RPS.Rock,
    B = RPS.Paper,
    C = RPS.Scissors
}

enum ResponseRPS
{
    X = RPS.Rock,
    Y = RPS.Paper,
    Z = RPS.Scissors
}