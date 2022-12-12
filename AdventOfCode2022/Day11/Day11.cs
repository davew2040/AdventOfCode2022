using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day11
{
    class Day11 : AocDay
    {
        public async Task Run()
        {
            var input = await File.ReadAllTextAsync("Day11/input.txt");
            var state = Parse(input);

            var inspections = CountInspections(state, 20);

            Console.WriteLine($"Monkey business = {inspections}");
        }
         
        ProblemState Parse(string text)
        {
            var chunks = text.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

            var result = chunks.Select(ParseMonkey).ToList();

            return new ProblemState
            {
                Monkeys = result.ToDictionary(x => x.Index)
            };
        }

        long CountInspections(ProblemState state, long rounds)
        {
            Dictionary<long, long> counter = new();

            RunRounds(state, rounds, (monkey, item) =>
            {
                if (!counter.ContainsKey(monkey.Index))
                {
                    counter[monkey.Index] = 0;
                }

                counter[monkey.Index]++;
            });

            var topTwo = counter.OrderByDescending(x => x.Value).Take(2).Select(x => x.Value);

            return topTwo.ElementAt(0) * topTwo.ElementAt(1);
        }

        void RunRounds(ProblemState state, long rounds, Action<Monkey, Item> itemInspected)
        {
            var commonDivisor = state.Monkeys.Select(x => x.Value.DivisibleBy).Aggregate(1L, (a, b) => a * b);

            for (long round = 1; round <= rounds; round++)
            {
                foreach (var monkey in state.Monkeys.Values)
                {
                    while (monkey.Items.Any())
                    {
                        var nextItem = monkey.Items.Dequeue();

                        itemInspected(monkey, nextItem);

                        long nextWorryLevel = GetNextWorryLevel(monkey, nextItem.WorryLevel);
                        long relievedWorryLevel = nextWorryLevel / 3;

                        if (relievedWorryLevel % monkey.DivisibleBy == 0)
                        {
                            state.Monkeys[monkey.TrueMonkey].Items.Enqueue(new Item(relievedWorryLevel));
                        }
                        else
                        {
                            state.Monkeys[monkey.FalseMonkey].Items.Enqueue(new Item(relievedWorryLevel));
                        }
                    }
                }
            }
        }

        long GetNextWorryLevel(Monkey monkey, long worryLevel)
        {
            long left = GetOperandValue(monkey.LeftOperand, worryLevel);
            long right = GetOperandValue(monkey.RightOperand, worryLevel);

            if (monkey.Operation == '*')
            {
                return left * right;
            }
            else if (monkey.Operation == '+')
            {
                return left + right;
            }

            throw new ArgumentException($"Invalid operation {monkey.Operation}");
        }

        long GetOperandValue(string operand, long worryLevel)
        {
            return operand == "old" ? worryLevel : long.Parse(operand);
        }

        Monkey ParseMonkey(string text)
        {
            var lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            var monkeyIndex = long.Parse(Regex.Match(lines[0], "\\d+").Value);
            var itemLevels = Regex.Matches(lines[1], "\\d+").ToArray().Select(x => long.Parse(x.Value));
            var operations = ParseOperation(lines[2]);
            var divisibleBy = long.Parse(Regex.Match(lines[3], "\\d+").Value);
            var trueMonkey = long.Parse(Regex.Match(lines[4], "\\d+").Value);
            var falseMonkey = long.Parse(Regex.Match(lines[5], "\\d+").Value);

            var newMonkey = new Monkey()
            {
                Index = monkeyIndex,
                LeftOperand = operations.leftOperand,
                RightOperand = operations.rightOperand,
                Operation = operations.operation[0],
                DivisibleBy = divisibleBy,
                TrueMonkey = trueMonkey,
                FalseMonkey = falseMonkey
            };

            foreach (var itemLevel in itemLevels)
            {
                newMonkey.Items.Enqueue(new Item(itemLevel));
            }

            return newMonkey;
        }

        (string leftOperand, string operation, string rightOperand) ParseOperation(string line)
        {
            var relevant = line.Split('=')[1];
            var split = relevant.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (split[0], split[1], split[2]);
        }

        class ProblemState
        {
            public Dictionary<long, Monkey> Monkeys { get; set; } = new();
        }

        class Monkey
        {
            public Queue<Item> Items { get; set; } = new Queue<Item>();
            public char Operation { get; set; }
            public string LeftOperand { get; set; }
            public string RightOperand { get; set; }
            public long DivisibleBy { get; set; }
            public long Index { get; set; }
            public long TrueMonkey { get; set; }
            public long FalseMonkey { get; set; }
        }

        class Item
        {
            public Item(long worryLevel)
            {
                this.WorryLevel = worryLevel;
            }

            public long WorryLevel { get; set; }
        }
    }
}
