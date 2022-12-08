using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day07
{
    public class Day07 : AocDay
    {
        public async Task Run()
        {
            var input = await File.ReadAllLinesAsync("Day07/input.txt");

            var root = Parse(input);

            int currentSize = GetSize(root);

            const int target = 30000000;
            const int total = 70000000;

            var largeEnough = Find(root, node => 
                {
                    return node.IsDirectory && total - (currentSize - this.GetSize(node)) >= target;
                });
            var smallest = largeEnough.OrderBy(n => GetSize(n)).First();

            Console.WriteLine($"smallest = {this.GetSize(smallest)}");
        }

        private FileSystemNode Parse(string[] input)
        {
            var result = new FileSystemNode("/", null);
            FileSystemNode current = result;

            int lineIndex = 0;

            while (lineIndex < input.Length)
            {
                var line = input[lineIndex];

                if (line.StartsWith("$"))
                {
                    var cmd = line.Substring(line.IndexOf(" ") + 1);

                    if (cmd.StartsWith("cd"))
                     {
                        var target = cmd.Split(' ')[1];

                        if (target == "/")
                        {
                            current = result;
                        }
                        else if (target == "..")
                        {
                            if (current.Parent == null)
                            {
                                throw new Exception("Can't move past root");
                            }
                            current = current.Parent;
                        }
                        else
                        {
                            AcknowledgeChildDirectory(current, target);
                            current = current.Children.First(c => c.Name == target);
                        }

                        lineIndex++;
                    }
                    else if (cmd.StartsWith("ls"))
                    {
                        int skip = DoLs(current, input, lineIndex+1);
                        lineIndex += skip + 1;
                    }
                }
            }

            return result;
        }

        private void VisitAll(FileSystemNode node, Action<FileSystemNode> action)
        {
            action(node);

            foreach (var child in node.Children)
            {
                VisitAll(child, action);
            }
        }

        private IEnumerable<FileSystemNode> Find(FileSystemNode node, Func<FileSystemNode, bool> pred)
        {
            List<FileSystemNode> match = new();

            this.VisitAll(node, node =>
            {
                if (pred(node))
                {
                    match.Add(node);
                }
            });

            return match;
        }

        private int GetSize(FileSystemNode current)
        {
            if (current.IsDirectory)
            {
                return current.Children.Sum(c => GetSize(c));
            }
            else
            {
                return current.Size;
            }
        }

        private int DoLs(FileSystemNode current, string[] lines, int index)
        {
            int count = 0;
            while (index < lines.Length && !lines[index].StartsWith("$"))
            {
                var nextline = lines[index];

                var split = nextline.Split(' ');

                if (split[0] == "dir")
                {
                    this.AcknowledgeChildDirectory(current, split[1]);
                }
                else
                {
                    this.AcknowledgeChildFile(current, split[1], int.Parse(split[0]));
                }

                index++;
                count++;
            }

            return count;
        }

        private void AcknowledgeChildDirectory(FileSystemNode node, string directory)
        {
            if (!node.Children.Any(c => c.Name == directory))
            {
                node.Children.Add(new FileSystemNode(directory, node));
            }
        }

        private void AcknowledgeChildFile(FileSystemNode node, string filename, int size)
        {
            var target = node.Children.FirstOrDefault(c => c.Name == filename);

            if (target == null)
            {
                var newNode = new FileSystemNode(filename, node);
                node.Children.Add(newNode);
                target = newNode;
            }

            target.Size = size;
        }

        public class FileSystemNode
        {
            public FileSystemNode(string name, FileSystemNode parent)
            {
                this.Parent = parent;
                this.Name = name;
            }

            public FileSystemNode Parent { get; set; }
            public List<FileSystemNode> Children { get; set; } = new List<FileSystemNode>();
            public string Name { get; set; }
            public int Size { get; set; } = 0;

            public bool IsDirectory => this.Size == 0;
        }
    }
}
