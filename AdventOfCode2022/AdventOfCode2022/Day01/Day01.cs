using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.D01
{
    public class Day01 : AocDay
    {
        public async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Day01/input.txt");

            var results = new List<int>();

            int current = 0;

            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    current += int.Parse(line);
                }
                else
                {
                    results.Add(current);
                    current = 0;
                }
            }

            if (current != 0)
            {
                results.Add(current);
            }

            Console.WriteLine(results.Max());

            var ordered = results.OrderByDescending(x => x);

            var topThree = ordered.Take(3);

            Console.WriteLine($"Top 3 = {topThree.Sum()}");

        }
    }
}
