using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SSIW2_CSP
{
    internal class FutoshikiDataLoader : IDataLoader
    {
        private readonly string _fileName;

        static readonly string Path = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
        private int Dimension { get; init; }

        public FutoshikiDataLoader(int dimension)
        {
            _fileName = $"{Path}\\Data\\futoshiki_{dimension}x{dimension}";
            Dimension = dimension;
        }

        public void LoadData(List<ILabel<int>> labels, List<IConstraint> constraints)
        {
            labels.Clear();
            constraints.Clear();
            List<List<ILabel<int>>> verticalLines = new List<List<ILabel<int>>>(Dimension);
            List<List<ILabel<int>>> horizontalLines = new List<List<ILabel<int>>>(Dimension);

            for (int i = 0; i < Dimension; i++)
            {
                horizontalLines.Add(new List<ILabel<int>>(Dimension));
                verticalLines.Add(new List<ILabel<int>>(Dimension));
            }
            string [] text = File.ReadAllText(_fileName).Split("\r\n");
            Variables(labels, verticalLines, horizontalLines, text);
            VerticalConstraints(constraints, horizontalLines, text);
            HorizontalConstraints(constraints, horizontalLines, text);
            LinesConstraints(constraints, verticalLines, horizontalLines);
        }

        private void Variables(List<ILabel<int>> labels, List<List<ILabel<int>>> verticalLines, List<List<ILabel<int>>> horizontalLines, string [] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                string line = text [i];
                if (i % 2 == 0)
                {
                    string [] stringLabels = line.Split('-', '<', '>').ToArray();
                    for (int j = 0; j < Dimension; j++)
                    {
                        IDomain<int> domain;
                        if (stringLabels [j] == "x")
                        {
                            domain = new SetDomain<int>(Enumerable.Range(1, Dimension));
                        }
                        else
                        {
                            domain = new ConstDomain<int>(int.Parse(stringLabels [j]));
                        }

                        Label<int> label = new Label<int>(domain);
                        labels.Add(label);
                        horizontalLines [i / 2].Add(label);
                        verticalLines [j].Add(label);
                    }
                }
            }
        }

        private void LinesConstraints(List<IConstraint> constraints, List<List<ILabel<int>>> verticalLines, List<List<ILabel<int>>> horizontalLines)
        {
            List<int?> numbers = new(verticalLines[0].Count);
            constraints.AddRange(horizontalLines.Union(verticalLines).Select(line => new Constraint(() =>
            {
                numbers.Clear();
                for (int i = 0; i < Dimension; i++)
                {
                    if (line[i].Value == null) continue;
                    if (numbers.Contains(line [i].Value))
                    {
                        return false;
                    }

                    numbers.Add(line [i].Value);

                }
                return true;
            })));
        }

        private void VerticalConstraints(List<IConstraint> constraints, List<List<ILabel<int>>> horizontalLines, string [] text)
        {
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
                                        (horizontalLines [row / 2] [col].Value ?? int.MinValue)
                                        <
                                        (horizontalLines [row / 2 + 1] [col].Value ?? int.MaxValue)
                                    )
                                );
                        }
                        else if (line [j] == '>')
                        {
                            constraints.Add(
                                new Constraint(
                                    () =>
                                        (horizontalLines [row / 2] [col].Value ?? int.MaxValue)
                                        >
                                        (horizontalLines [row / 2 + 1] [col].Value ?? int.MinValue)
                                    )
                                );
                        }
                    }
                }
            }
        }

        private static void HorizontalConstraints(List<IConstraint> constraints, List<List<ILabel<int>>> horizontalLines, string [] text)
        {
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
                                        (horizontalLines [row / 2] [col].Value ?? int.MinValue)
                                        <
                                        (horizontalLines [row / 2] [col + 1].Value ?? int.MaxValue)
                                    )
                                );
                            j++;
                        }
                        else if (c == '>')
                        {
                            constraints.Add(
                                new Constraint(
                                    () =>
                                        (horizontalLines [row / 2] [col].Value ?? int.MaxValue)
                                        >
                                        (horizontalLines [row / 2] [col + 1].Value ?? int.MinValue)
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