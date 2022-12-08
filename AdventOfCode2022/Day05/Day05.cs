using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day05
{
    internal class Day05 : AocDay
    {
        public async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Day05/input.txt");

            var parsed = Parse(lines);

            ApplyMoves(parsed.stacks, parsed.moves);

            foreach (var stack in parsed.stacks.stacks)
            {
                Console.Write(stack.First());
            }
        }

        void ApplyMoves(Stacks data, IEnumerable<Move> moves)
        {
            foreach (var move in moves)
            {
                ApplyMove(data, move);
            }
        }

        void ApplyMove(Stacks data, Move move)
        {
            for (int i=0; i<move.Count; i++)
            {
                data.stacks[move.DestinationIndex-1].Push(data.stacks[move.SourceIndex-1].Pop());
            }
        }

        (Stacks stacks, IEnumerable<Move> moves) Parse(string[] lines)
        {
            int splitIndex = Array.FindIndex(lines, 0, lines.Length, line => string.IsNullOrEmpty(line));
            var numberOfQueues = int.Parse(lines[splitIndex - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());

            var result = new Stacks(numberOfQueues);

            for (int lineIndex = splitIndex-2; lineIndex >= 0; lineIndex--) 
            { 
                var line = lines[lineIndex];

                var currentStack = 0;
                var jumpSize = "] [".Length + 1;

                for (int linePosition = 1; linePosition < line.Length; linePosition += jumpSize, currentStack++)
                {
                    if (line[linePosition] != ' ')
                    {
                        result.stacks[currentStack].Push(line[linePosition]);
                    }
                }
            }

            var moves = lines[(splitIndex + 1)..].Select(ParseMove).ToList();

            return (result, moves);
        }



        Move ParseMove(string line)
        {
            var size = int.Parse(SubstringBetween(line, "move ", " from"));
            var from = int.Parse(SubstringBetween(line, "from ", " to"));
            var to = int.Parse(line.Substring(line.IndexOf("to ") + "to ".Length));

            return new Move
            {
                Count = size,
                SourceIndex = from,
                DestinationIndex = to
            };
        }

        string SubstringBetween(string line, string prefix, string postfix)
        {
            int start = line.IndexOf(prefix) + prefix.Length;
            int end = line.IndexOf(postfix);

            return line.Substring(start, end - start);
        }

        class Stacks
        {
            public Stacks(int size)
            {
                stacks = new Stack<char>[size];

                for (int i=0; i<size; i++)
                {
                    stacks[i] = new Stack<char>();
                }
            }

            public Stack<char>[] stacks;
        }

        class Move
        {
            public int Count;
            public int SourceIndex;
            public int DestinationIndex;
        }
    }
}
