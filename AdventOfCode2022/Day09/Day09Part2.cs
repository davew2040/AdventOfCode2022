using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day09
{
    internal class Day09Part2 : AocDay
    {
        private const int RopeSize = 10;

        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day09/input.txt");

            var moves = Parse(input);

            var problemState = new ProblemState(RopeSize);

            var uniquetail = GetPositionsAtLeastOnce(problemState, moves);

            Console.WriteLine($"Unique tail positions = {uniquetail}");
        }

        int GetPositionsAtLeastOnce(ProblemState initialState, IEnumerable<Move> moves)
        {
            Dictionary<Point, int> tailTracker = new()
            {
                { new Point(0,0), 1 }
            };

            ApplyMoves(initialState, moves, (state) =>
            {
                if (!tailTracker.ContainsKey(state.rope.Last()))
                {
                    tailTracker.Add(state.rope.Last(), 0);
                }

                tailTracker[state.rope.Last()]++;
            });

            return tailTracker.Keys.Count;
        }

        void ApplyMoves(ProblemState state, IEnumerable<Move> moves, Action<ProblemState> visitor)
        {
            foreach (var move in moves)
            {
                ApplyMove(state, move, visitor);
            }
        }

        void ApplyMove(ProblemState state, Move move, Action<ProblemState> visitor)
        {
            for (int i=0; i<move.Repeats; i++)
            {
                List<Point> newPoints = new();

                var movedHead = MovePoint(state.rope.First(), move);
                newPoints.Add(movedHead);

                for (int j=1; j<state.rope.Count(); j++)
                {
                    var movedNext = GetNextTailPosition(newPoints.Last(), state.rope.ElementAt(j));
                    newPoints.Add(movedNext);
                }

                state.rope = newPoints;

                visitor(state);
            }
        }

        Point GetNextTailPosition(Point head, Point tail)
        {
            var distY = head.Y - tail.Y;
            var distX = head.X - tail.X;

            if (Math.Abs(distX) >= 2 || Math.Abs(distY) >= 2)
            {
                return new Point(tail.X + ClampInt(distX), tail.Y + ClampInt(distY));
            }

            return tail;
        }

        int ClampInt(int value)
        {
            if (value >= 1)
            {
                return 1;
            }
            else if (value <= -1)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        Point MovePoint(Point point, Move move)
        {
            if (move.Direction == 'U')
            {
                return new Point(point.X, point.Y + 1);
            }
            else if (move.Direction == 'D')
            {
                return new Point(point.X, point.Y - 1);
            }
            else if (move.Direction == 'L')
            {
                return new Point(point.X-1, point.Y);
            }
            else if (move.Direction == 'R')
            {
                return new Point(point.X+1, point.Y);
            }

            throw new ArgumentException($"Invalid direction {move.Direction}");
        }

        IEnumerable<Move> Parse(string[] lines)
        {
            return lines.Select(line =>
            {
                var split = line.Split(' ');
                return new Move
                {
                    Direction = split[0][0],
                    Repeats = int.Parse(split[1])
                };
            });
        }

        class ProblemState
        {
            public ProblemState(int ropeSize)
            {
                var initialValues = new List<Point>();

                for (int i=1; i<=ropeSize; i++)
                {
                    initialValues.Add(new Point());
                }

                this.rope = initialValues;
            }

            public IEnumerable<Point> rope;
        }

        struct Point
        {
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override int GetHashCode() => (X, Y).GetHashCode();

            public int X;
            public int Y;
        }

        struct Move
        {
            public char Direction { get; set; }
            public int Repeats { get; set; }
        }
    }
}
