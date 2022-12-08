using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day04
{
    public class Day04 : AocDay
    {
        public async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Day04/input.txt");

            var parsed = Parse(lines);

            var count = parsed.Where(HasAnyOverlap).Count();

            Console.WriteLine($"count = {count}");
        }

        bool Contains(Range first, Range second)
        {
            return first.Start <= second.Start && first.End >= second.End;
        }

        bool HasAnyOverlap(RangePair pair)
        {
            return Contains(pair.First, pair.Second) || Contains(pair.Second, pair.First)
                || PointIsWithin(pair.First.Start, pair.Second) || PointIsWithin(pair.First.End, pair.Second)
                || PointIsWithin(pair.Second.Start, pair.First) || PointIsWithin(pair.Second.End, pair.First);
        }

        bool PointIsWithin(int point, Range range)
        {
            return point >= range.Start && point <= range.End;
        }

        IEnumerable<RangePair> Parse(string[] lines)
        {
            return lines.Select(line =>
            {
                var split = line.Split(",");
                var one = ParseRange(split[0]);
                var two = ParseRange(split[1]);

                return new RangePair
                {
                    First = one,
                    Second = two
                };
            });
        }

        Range ParseRange(string expression)
        {
            var split = expression.Split("-");
            var one = int.Parse(split[0]);
            var two = int.Parse(split[1]);

            return new Range
            {
                Start = one,
                End = two
            };
        }

        public readonly record struct Range(int Start, int End);

        public readonly record struct RangePair(Range First, Range Second);

        
    }
}
