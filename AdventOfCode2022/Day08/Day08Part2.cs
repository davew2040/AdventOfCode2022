using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day08
{
    internal class Day08Part2 : AocDay
    {
        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day08/input.txt");

            var parsed = Parse(input);

            int highestScenic = GetAll(parsed).Max(p => GetScenicScore(p.Row, p.Col, parsed));
            Console.WriteLine($"total = {highestScenic}");
        }

        int[,] Parse(string[] lines)
        {
            var width = lines.Max(x => x.Length);
            var height = lines.Length;

            var result = new int[width, height];

            for (int r=0; r<height; r++)
            {
                for (int c=0; c<width; c++)
                {
                    result[r, c] = lines[r][c] - '0';
                }
            }

            return result;
        }

        private IEnumerable<Point> GetLeft(int currentRow, int currentCol, int[,] forest)
        {
            List<Point> result = new List<Point>();

            for (int c = currentCol - 1; c >= 0; c--)
            {
                result.Add(new Point(currentRow, c));
            }

            return result;
        }

        private IEnumerable<Point> GetRight(int currentRow, int currentCol, int[,] forest)
        {
            List<Point> result = new List<Point>();

            for (int c = currentCol + 1; c < forest.GetLength(1); c++)
            {
                result.Add(new Point(currentRow, c));
            }

            return result;
        }

        private IEnumerable<Point> GetBottom(int currentRow, int currentCol, int[,] forest)
        {
            List<Point> result = new List<Point>();

            for (int r = currentRow + 1; r < forest.GetLength(0); r++)
            {
                result.Add(new Point(r, currentCol));
            }

            return result;
        }

        private IEnumerable<Point> GetTop(int currentRow, int currentCol, int[,] forest)
        {
            List<Point> result = new List<Point>();

            for (int r = currentRow - 1; r >= 0; r--)
            {
                result.Add(new Point(r, currentCol));
            }

            return result;
        }

        private IEnumerable<Point> GetAll(int[,] forest)
        {
            List<Point> result = new List<Point>();

            for (int r = 0; r < forest.GetLength(0); r++)
            {
                for (int c = 0; c < forest.GetLength(1); c++)
                {
                    result.Add(new Point(r, c));
                }
            }

            return result;
        }

        private int GetScenicScore(int currentRow, int currentCol, int[,] forest)
        {
            int mult = 1;

            List<IEnumerable<Point>> directions = new List<IEnumerable<Point>>()
            {
                GetLeft(currentRow, currentCol, forest),
                GetRight(currentRow, currentCol, forest),
                GetTop(currentRow, currentCol, forest),
                GetBottom(currentRow, currentCol, forest)
            };

            foreach (var span in directions)
            {
                mult *= GetVisibleTrees(currentRow, currentCol, span, forest);
            }

            return mult;
        }

        private int GetVisibleTrees(int currentRow, int currentCol, IEnumerable<Point> span, int[,] forest)
        {
            int count = 0;
            foreach (var spanPoint in span)
            {
                count++;

                if (forest[spanPoint.Row, spanPoint.Col] >= forest[currentRow, currentCol]) {
                    break;
                }
            }
            return count;
        }

        private struct Point
        {
            public Point(int row, int col)
            {
                this.Row = row;
                this.Col = col;
            }

            public int Row;
            public int Col;
        }
    }
}
