using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10
{
    internal class Day10 : AocDay
    {
        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day10/input.txt");
            var pendingInstructions = new Queue<Instruction>(input.Select(Parse));
            var activeInstructions = new List<Instruction>();

            int currentCycle = 1;
            Instruction activeInstruction = null;
            List<int> samples = new();
            var state = new RegisterState();

            while (pendingInstructions.Any() || activeInstruction != null)
            {
                if (activeInstruction == null && pendingInstructions.Any())
                {
                    activeInstruction = pendingInstructions.Dequeue();
                }

                Console.WriteLine($"{currentCycle} = {state.X}");

                if ((currentCycle - 20) % 40 == 0)
                {
                    samples.Add(state.X * currentCycle);
                    Console.WriteLine($"*** testX = {state.X}");
                }

                if (activeInstruction is Noop)
                {
                    activeInstruction = null;
                }
                else if (activeInstruction is Add)
                {
                    var asAdd = activeInstruction as Add;
                    asAdd.RemainingCycles--;
                    if (asAdd.RemainingCycles == 0)
                    {
                        state.X += asAdd.Value;
                        activeInstruction = null;
                    }
                }

                currentCycle++;
            }

            Console.WriteLine($"FINAL: {currentCycle} = {state.X}");

            Console.WriteLine($"Sum = {samples.Sum()}");
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
