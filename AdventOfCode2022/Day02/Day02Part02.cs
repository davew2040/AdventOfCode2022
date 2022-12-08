using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day02
{
    public class Day02Part02 : AocDay
    {
        private int[,] OutcomeMatrix = new int[3, 3];

        public Day02Part02()
        {
            OutcomeMatrix[(int)Play.Rock, (int)Play.Rock] = 0;
            OutcomeMatrix[(int)Play.Rock, (int)Play.Paper] = -1;
            OutcomeMatrix[(int)Play.Rock, (int)Play.Scissors] = 1;

            OutcomeMatrix[(int)Play.Paper, (int)Play.Rock] = 1;
            OutcomeMatrix[(int)Play.Paper, (int)Play.Paper] = 0;
            OutcomeMatrix[(int)Play.Paper, (int)Play.Scissors] = -1;

            OutcomeMatrix[(int)Play.Scissors, (int)Play.Rock] = -1;
            OutcomeMatrix[(int)Play.Scissors, (int)Play.Paper] = 1;
            OutcomeMatrix[(int)Play.Scissors, (int)Play.Scissors] = 0;
        }

        public async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Day02/input.txt");

            var rounds = Parse(lines);

            var result = rounds.Select(round => ScoreRound(round)).Sum();

            Console.WriteLine($"Final score = {result}");
        }

        private int ScoreRound(Round round)
        {
            var outcome = OutcomeMatrix[(int)round.Player2, (int)round.Player1];

            var outcomeScore = ScoreOutcome[outcome];
            var playScore = PlayScore[round.Player2];

            return outcomeScore + playScore;
        }

        private IEnumerable<Round> Parse(string[] lines)
        {
            return lines.Select(ParseRound);
        }

        private Round ParseRound(string line)
        {
            var split = line.Split(new char[] { ' ' });

            var p1Play = CharToPlay[split[0].ElementAt(0)];
            var expected = CharToExpected[split[1].ElementAt(0)];

            Play p2Play;

            if (expected == RoundResult.Tie)
            {
                p2Play = (Play)IndexWhereEqual(OutcomeMatrix, (int)p1Play, 0);
            }
            else if (expected == RoundResult.Loss)
            {
                p2Play = (Play)IndexWhereEqual(OutcomeMatrix, (int)p1Play, -1);
            }
            else
            {
                p2Play = (Play)IndexWhereEqual(OutcomeMatrix, (int)p1Play, 1);
            }

            var round = new Round
            {
                Player1 = p1Play,
                Player2 = p2Play
            };

            return round;
        }

        private int IndexWhereEqual(int[,] array2d, int row, int value)
        {
            for (int c = 0; c<array2d.GetLength(0); c++)
            {
                if (array2d[c,row] == value)
                {
                    return c;
                }
            }

            throw new Exception("Invalid row result");
        }

        private enum Play
        {
            Rock = 0,
            Paper,
            Scissors
        }

        private enum RoundResult
        {
            Win, 
            Loss,
            Tie
        }

        private class Round
        {
            public Play Player1;
            public Play Player2;
        }

        private Dictionary<char, Play> CharToPlay = new Dictionary<char, Play>()
        {
            { 'A', Play.Rock },
            { 'B', Play.Paper },
            { 'C', Play.Scissors },
            { 'X', Play.Rock },
            { 'Y', Play.Paper },
            { 'Z', Play.Scissors }
        };

        private Dictionary<char, RoundResult> CharToExpected = new Dictionary<char, RoundResult>()
        {
            { 'X', RoundResult.Loss },
            { 'Y', RoundResult.Tie },
            { 'Z', RoundResult.Win }
        };

        private Dictionary<Play, int> PlayScore = new Dictionary<Play, int>()
        {
            { Play.Rock, 1 },
            { Play.Paper, 2 },
            { Play.Scissors, 3 }
        };

        private Dictionary<int, int> ScoreOutcome = new Dictionary<int, int>()
        {
            { -1, 0 },
            { 0, 3 },
            { 1, 6 }
        };
    }
}
