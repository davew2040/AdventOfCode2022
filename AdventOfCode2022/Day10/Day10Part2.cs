using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10
{
    internal class Day10Part2 : AocDay
    {
        const int TotalPixels = 240;

        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day10/input.txt");
            var instructions = input.Select(Parse);

            char[] pixels = new char[TotalPixels];
            int currentTarget = 0;

            ProduceCycleValues(instructions, (cycle, x) =>
            {
                var span = new List<int>()
                {
                    x-1, x, x+1
                };
                if (span.Contains(currentTarget % 40))
                {
                    pixels[currentTarget] = '*';
                }
                else
                {
                    pixels[currentTarget] = '.';
                }
                currentTarget++;
            });


            DrawPixels(pixels);
        }

        void DrawPixels(char[] drawPixels)
        {
            for (int i=0; i<drawPixels.Length; i++)
            {
                if (i > 0 && i % 40 == 0)
                {
                    Console.WriteLine("");
                }

                Console.Write(drawPixels[i] == '\0' ? " " : drawPixels[i]);
            }

            Console.WriteLine("\n");
        }

        void ProduceCycleValues(IEnumerable<Instruction> instructions, Action<int, int> xValuesProcessor)
        {
            var pendingInstructions = new Queue<Instruction>(instructions.Select(x => x));
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

                xValuesProcessor(currentCycle, state.X);

                if ((currentCycle - 20) % 40 == 0)
                {
                    samples.Add(state.X * currentCycle);
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
