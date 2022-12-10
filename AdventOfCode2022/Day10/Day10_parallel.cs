using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10
{
    internal class Day10_parallel : AocDay
    {
        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day10/sample.txt");
            var pendingInstructions = new Queue<Instruction>(input.Select(Parse));
            var activeInstructions = new List<Instruction>();

            int currentCycle = 1;
            List<int> samples = new();
            var state = new RegisterState();

            while (pendingInstructions.Any() || activeInstructions.Any())
            {
                if (pendingInstructions.Any())
                {
                    activeInstructions.Add(pendingInstructions.Dequeue());
                }

                Console.WriteLine($"{currentCycle} = {state.X}");

                HashSet<Instruction> toRemove = new();

                foreach (var instruction in activeInstructions)
                {
                    if ((currentCycle - 20) % 40 == 0)
                    {
                        Console.WriteLine($"*** testX = {state.X}");
                    }

                    if (instruction is Noop)
                    {
                        toRemove.Add(instruction);
                    }
                    else if (instruction is Add)
                    {
                        var asAdd = instruction as Add;
                        asAdd.RemainingCycles--;
                        if (asAdd.RemainingCycles == 0)
                        {
                            state.X += asAdd.Value;
                            toRemove.Add(asAdd);
                        }
                    }
                }

                activeInstructions.RemoveAll(instruction => toRemove.Contains(instruction));
                currentCycle++;
            }

            Console.WriteLine($"FINAL: {currentCycle} = {state.X}");
        }

        Instruction Parse(string line)
        {
            if (line.StartsWith("noop"))
            {
                return new Noop();
            }
            else if (line.StartsWith("addx"))
            {
                return new Add()
                {
                    RemainingCycles = 2,
                    Value = int.Parse(line.Split(" ")[1])
                };
            }
            else
            {
                throw new Exception($"Unrecognized instruction {line}");
            }
        }

        abstract class Instruction
        {
        }

        class Noop : Instruction
        {

        }
        
        class Add : Instruction
        {
            public int Value;
            public int RemainingCycles;
        }

        class RegisterState
        {
            public int X = 1;
        }
    }
}
