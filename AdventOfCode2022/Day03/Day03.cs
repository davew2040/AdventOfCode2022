using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day03
{
    public class Day03 : AocDay
    {
        public async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Day03/input.txt");

            var sacks = Parse(lines);

            var uniques = sacks.Select(GetUniqueItem);
            var values = uniques.Select(GetItemValue);

            var sum = values.Sum();

            Console.WriteLine($"sum = {sum}");
        }

        char GetUniqueItem(TwoSacks sacks)
        {
            var firstSet = sacks.One.items.ToHashSet();

            var duplicate = sacks.Two.items.First(c => firstSet.Contains(c));

            return duplicate;
        }

        int GetItemValue(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return c - 'a' + 1;
            }
            if (c >= 'A' && c <= 'Z')
            {
                return c - 'A' + 27;
            }

            throw new ArgumentException($"{c} is out of range");
        }

        private IEnumerable<TwoSacks> Parse(string[] lines)
        {
            return lines.Select(ParseLine);
        }

        private TwoSacks ParseLine(string line)
        {
            var firsthalf = line.Substring(0, line.Length / 2);
            var secondhalf = line.Substring(line.Length / 2, line.Length / 2);

            return new TwoSacks
            {
                One = new Sack { items = firsthalf.ToArray() },
                Two = new Sack { items = secondhalf.ToArray() },
            };
        }

        class Sack
        {
            public char[] items;
        }

        class TwoSacks
        {
            public Sack One;
            public Sack Two;
        }
    }
}
