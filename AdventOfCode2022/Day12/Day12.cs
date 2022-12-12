using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day12
{
    internal class Day12 : AocDay
    {
        public async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Day12/input.txt");
            var grid = Parse(lines);
            var path = FindPath(grid);

            Console.WriteLine($"Path size = {path.Count()}");
        }

        private static readonly Point Left = new Point(0, -1);
        private static readonly Point Right = new Point(0, 1);
        private static readonly Point Up = new Point(-1, 0);
        private static readonly Point Down = new Point(1, 0);

        private readonly List<Point> Directions = new List<Point>()
        {
            Left,
            Right,
            Up,
            Down
        };

        List<Point> FindPath(ProblemState state)
        {
            Queue<Point> processing = new();

            processing.Enqueue(new Point(0, 0));
            HashSet<Point> visited = new();
            Dictionary<Point, Point> predecessor = new();

            while (processing.Any())
            {
                var currentPoint = processing.Dequeue();

                foreach (var direction in Directions)
                {
                    var nextPoint = new Point(currentPoint.R + direction.R, currentPoint.C + direction.C);

                    if (!IsValid(nextPoint, state) || visited.Contains(nextPoint))
                    {
                        continue;
                    }

                    if (CanMove(currentPoint, nextPoint, state))
                    {
                        predecessor[nextPoint] = currentPoint;

                        if (state.Grid[nextPoint.R, nextPoint.C] == 'E')
                        {
                            return GetPath(new Point(0, 0), nextPoint, predecessor);
                        }

                        visited.Add(nextPoint);
                        processing.Enqueue(nextPoint);
                    }
                }
            }

            throw new Exception("No valid path found");
        }

        private List<Point> GetPath(Point firstPoint, Point lastPoint, Dictionary<Point, Point> predecessors)
        {
            List<Point> path = new();
            var currentPoint = lastPoint;

            while (currentPoint.GetHashCode() != firstPoint.GetHashCode())
            {
                path.Add(currentPoint);

                currentPoint = predecessors[currentPoint];
            }

            return path;
        }

        private void PrintPath(List<Point> path, ProblemState state)
        {
            for (int r=0; r<state.Grid.GetLength(0); r++)
            {
                for (int c=0; c<state.Grid.GetLength(1); c++)
                {
                    if (path.Contains(new Point(r, c)))
                    {
                        Console.Write((char)(path.IndexOf(new Point(r, c)) + 'a'));
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("");
            }
        }

        private char GetValue(Point p, ProblemState state) 
        { 
            var value = state.Grid[p.R, p.C]; 

            if (value == 'S')
            {
                return 'a';
            }
            else if (value == 'E')
            {
                return 'z';
            }
            else
            {
                return value;
            }
        }

        private bool CanMove(Point source, Point destination, ProblemState state)
        {
            var sourceHeight = GetValue(source, state);
            var destHeight = GetValue(destination, state);

            return destHeight - sourceHeight <= 1;
        }

        private bool IsValid(Point p, ProblemState state)
        {
            return p.R >= 0 && p.C >= 0 && p.R < state.Grid.GetLength(0) && p.C < state.Grid.GetLength(1);
        }

        ProblemState Parse(string[] lines)
        {
            var totalCols = lines.Max(l => l.Length);
            var totalRows = lines.Length;

            var grid = new char[totalRows, totalCols];

            for (int r = 0; r < totalRows; r++)
            {
                for (int c = 0; c < totalCols; c++)
                {
                    grid[r, c] = lines[r][c];
                }
            }

            return new ProblemState
            {
                Grid = grid
            };
        }

        class ProblemState
        {
            public char[,] Grid { get; set; }
        }

        struct Point
        {
            public Point(int r, int c)
            {
                this.R = r;
                this.C = c;
            }

            public int R;
            public int C;

            public override int GetHashCode() => (R, C).GetHashCode();
        }
    }
}
