using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day03
{
    public class Day03Part02 : AocDay
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

        char GetUniqueItem(SackGroup sacks)
        {
            var dict = new Dictionary<char, int>();

            foreach (var sack in sacks.sacks.Select(GetUniqueItemsInSack))
            {
                foreach (var c in sack)
                {
                    if (!dict.ContainsKey(c))
                    {
                        dict[c] = 0;
                    }

                    dict[c]++;
                }
            }

            var max = dict.Max(kvp => kvp.Value);

            return dict.First(kvp => kvp.Value == max).Key;
        }

        HashSet<char> GetUniqueItemsInSack(Sack s)
        {
            var result = new HashSet<char>();

            foreach (var c in s.items)
            {
                if (!result.Contains(c))
                {
                    result.Add(c);
                }
            }

            return result;
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

        private IEnumerable<SackGroup> Parse(string[] lines)
        {
            var groups = new List<SackGroup>();

            for (int l = 0; l < lines.Length; l += 3)
            {
                if (string.IsNullOrEmpty(lines[l]))
                {
                    break;
                }

                groups.Add(
                    ParseLineGroup(new string[] {
                        lines[l],
                        lines[l+1],
                        lines[l+2]
                })); 
            }

            return groups;
        }

        private SackGroup ParseLineGroup(string[] lines)
        {
            return new SackGroup
            {
                sacks = lines.Select(l => new Sack { items = l.ToArray() })
            };
        }

        class Sack
        {
            public char[] items;
        }

        class SackGroup
        {
            public IEnumerable<Sack> sacks;
        }
    }
}
