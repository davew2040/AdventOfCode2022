using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day06
{
    internal class Day06 : AocDay
    {
        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day06/sample.txt");

            var markerPosition = FindFirstMarker(input[0]);

            Console.WriteLine($"Marker position = {markerPosition}");
        }

        private int FindFirstMarker(string buffer)
        {
            Queue<char> inputQueue = new Queue<char>();
            Dictionary<char, int> counter = new();

            for (int i=0; i<4; i++)
            {
                inputQueue.Enqueue(buffer[i]);
                Increment(counter, buffer[i]);
            }

            if (IsMarker(counter))
            {
                return 4;
            }

            for (int i=4; i<buffer.Length; i++)
            {
                var removing = inputQueue.Dequeue();
                inputQueue.Enqueue(buffer[i]);

                Decrement(counter, removing);
                Increment(counter, buffer[i]);

                if (IsMarker(counter))
                {
                    return i+1;
                }
            }

            throw new Exception("No marker found");
        }

        private bool IsMarker(Dictionary<char, int> counter)
        {
            foreach (var kvp in counter)
            {
                if (kvp.Value > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private void Increment(Dictionary<char, int> counter, char value)
        {
            if (!counter.ContainsKey(value))
            {
                counter[value] = 0;
            }

            counter[value]++;
        }

        private void Decrement(Dictionary<char, int> counter, char value)
        {
            if (!counter.ContainsKey(value))
            {
                throw new ArgumentException($"Can't decrement value {value}");
            }

            counter[value]--;

            if (counter[value] == 0)
            {
                counter.Remove(value);
            }
        }
    }
}
