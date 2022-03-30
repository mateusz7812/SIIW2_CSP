using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SSIW2_CSP
{
    internal class FutoshikiDataLoader : IDataLoader
    {
        private readonly string file_name;

        static string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private int Dimension { get; init; }

        public FutoshikiDataLoader(int dimension)
        {
            file_name = $"{path}\\Data\\futoshiki_{dimension}x{dimension}";
            this.Dimension = dimension;
        }

        public void LoadData(List<ILabel<int>> labels, List<IConstraint> constraints)
        {
            labels.Clear();
            constraints.Clear();
            List<List<ILabel<int>>> vertical_lines = new List<List<ILabel<int>>>(Dimension);
            List<List<ILabel<int>>> horizontal_lines = new List<List<ILabel<int>>>(Dimension);

            for (int i = 0; i < Dimension; i++)
            {
                horizontal_lines.Add(new List<ILabel<int>>(Dimension));
                vertical_lines.Add(new List<ILabel<int>>(Dimension));
            }
            string [] text = File.ReadAllText(file_name).Split("\r\n");
            for (int i = 0; i < text.Length; i++)
            {
                string line = text [i];
                if (i % 2 == 0)
                {
                    string [] string_labels = line.Split('-', '<', '>').ToArray();
                    for (int j = 0; j < Dimension; j++)
                    {
                        IDomain<int> domain;
                        if (string_labels [j] == "x")
                        {
                            domain = new SetDomain<int>(Enumerable.Range(1, Dimension));
                        }
                        else
                        {
                            domain = new ConstDomain<int>(int.Parse(string_labels [j]));
                        }

                        Label<int> label = new Label<int>(domain);
                        labels.Add(label);
                        horizontal_lines [i/2].Add(label);
                        vertical_lines [j].Add(label);
                    }
                }
            }
            constraints.AddRange(horizontal_lines.Union(vertical_lines).Select(line => new Constraint(() =>
            {
                List<int> free_numbers = Enumerable.Range(1, Dimension).ToList();
                for (int i = 0; i < Dimension; i++)
                {
                    if (line [i].Value != null)
                    {
                        if (free_numbers.Contains((int) line [i].Value))
                        {
                            free_numbers.Remove((int) line [i].Value);
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                return true;
            })));

            for (int i = 0; i < text.Length; i++)
            {
                string line = text [i];
                if (i % 2 == 1)
                {
                    for (int j = 0; j < Dimension; j++)
                    {
                        int row = i;
                        int col = j;
                        if (line [j] == '<')
                        {
                            constraints.Add(
                                new Constraint(
                                    () =>
                                        (horizontal_lines [(row / 2)] [col].Value ?? int.MinValue)
                                        <
                                        (horizontal_lines [(row / 2 + 1)] [col].Value ?? int.MaxValue)
                                    )
                                );
                        }
                        else if (line [j] == '>')
                        {
                            constraints.Add(
                                new Constraint(
                                    () =>
                                        (horizontal_lines [(row / 2)] [col].Value ?? int.MaxValue)
                                        >
                                        (horizontal_lines [(row / 2) + 1] [col].Value ?? int.MinValue)
                                    )
                                );
                        }
                    }
                }
            }

            for (int i = 0; i < text.Length; i++)
            {
                string line = text [i];
                if (i % 2 == 0)
                {
                    int j = 0;
                    foreach (char c in line)
                    {
                        int row = i;
                        int col = j;
                        if (c == '-')
                        {
                            j++;
                        }
                        else if (c == '<')
                        {
                            constraints.Add(
                                new Constraint(
                                    () =>
                                        (horizontal_lines [row / 2] [col].Value ?? int.MinValue)
                                        <
                                        (horizontal_lines [row / 2] [col + 1].Value ?? int.MaxValue)
                                    )
                                );
                            j++;
                        }
                        else if (c == '>')
                        {
                            constraints.Add(
                                new Constraint(
                                    () =>
                                        (horizontal_lines [row / 2] [col].Value ?? int.MaxValue)
                                        >
                                        (horizontal_lines [row / 2] [col + 1].Value ?? int.MinValue)
                                    )
                                );
                            j++;
                        }

                    }
                }
            }
        }
    }
}