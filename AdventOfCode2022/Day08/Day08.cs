using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day08
{
    internal class Day08 : AocDay
    {
        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day08/input.txt");

            var parsed = Parse(input);

            int interiorVisible = CountVisible(parsed);

            int exteriorVisible = parsed.GetLength(0) * 2 + parsed.GetLength(1) * 2 - 4;

            Console.WriteLine($"total = {interiorVisible + exteriorVisible}");
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

        private int CountVisible(int[,] forest)
        {
            int count = 0;

            for (int r=1; r<forest.GetLength(0)-1; r++)
            {
                for (int c = 1; c < forest.GetLength(1) - 1; c++)
                {
                    if (IsVisible(r, c, forest))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool IsVisible(int currentRow, int currentCol, int[,] forest)
        {
            int maxLeft = int.MinValue;

            for (int c=0; c<currentCol; c++)
            {
                maxLeft = Math.Max(maxLeft, forest[currentRow, c]);
            }
            if (maxLeft < forest[currentRow, currentCol])
            {
                return true;
            }

            int maxRight = int.MinValue;
            for (int c = currentCol+1; c < forest.GetLength(1); c++)
            {
                maxRight = Math.Max(maxRight, forest[currentRow, c]);
            }
            if (maxRight < forest[currentRow, currentCol])
            {
                return true;
            }

            int maxUp = int.MinValue;
            for (int r = 0; r < currentRow; r++)
            {
                maxUp = Math.Max(maxUp, forest[r, currentCol]);
            }
            if (maxUp < forest[currentRow, currentCol])
            {
                return true;
            }

            int maxDown = int.MinValue;
            for (int r = currentRow+1; r < forest.GetLength(0); r++)
            {
                maxDown = Math.Max(maxDown, forest[r, currentCol]);
            }
            if (maxDown < forest[currentRow, currentCol])
            {
                return true;
            }

            return false;
        }
    }
}
