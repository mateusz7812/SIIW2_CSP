using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Domains;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.DataLoaders
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
            int labelIdCounter = 0;
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

                        LabelWithDeletedValues<int> label = new LabelWithDeletedValues<int>(new Label<int>(domain), labelIdCounter);
                        labels.Add(label);
                        horizontalLines [i / 2].Add(label);
                        verticalLines [j].Add(label);

                        labelIdCounter++;
                    }
                }
            }
        }

        private void LinesConstraints(List<IConstraint> constraints, List<List<ILabel<int>>> verticalLines, List<List<ILabel<int>>> horizontalLines)
        {
            horizontalLines.Union(verticalLines)
                .ToList().ForEach(line =>
                {
                    List<int?> numbers = new List<int?>(Dimension);
                    Dictionary<int, List<int>> removedValues = new();
                    var constraint = new ConstraintWithDomainChecking<int>(() =>
                    {
                        numbers.Clear();
                        for (int i = 0; i < Dimension; i++)
                        {
                            if (line[i].Value == null) continue;
                            if (numbers.Contains(line[i].Value))
                            {
                                return false;
                            }

                            numbers.Add(line[i].Value);
                        }

                        return true;
                    },
                        current =>
                        {
                            removedValues.Clear();
                            if (!line.Contains(current)) 
                                return removedValues;
                            
                            foreach (var l in line)
                            {
                                if (l == current || l.Value is not null) continue;
                                if(l is LabelWithDeletedValues<int> label)
                                    removedValues[label.ID] = l.RemoveFromDomain(v => v == current.Value);
                            }

                            return removedValues;
                        });
                    line.ForEach(label => label.Constraints.Add(constraint));
                });
        }

        private void VerticalConstraints(List<IConstraint> constraints, List<List<ILabel<int>>> horizontalLines, string [] text)
        {
            //Dictionary<ILabel<int>, List<int>> removedValues = new();
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
                            var constraint = new Constraint(
                                    () =>
                                        (horizontalLines[row / 2][col].Value ?? int.MinValue)
                                        <
                                        (horizontalLines[row / 2 + 1][col].Value ?? int.MaxValue)
                                        );
                            horizontalLines[row / 2][col].Constraints.Add(constraint);
                            horizontalLines[row / 2 + 1][col].Constraints.Add(constraint);
                            //      , (label) =>
                            //  {
                            //      removedValues.Clear();
                            //      if (label == horizontalLines[row / 2][col])
                            //      {
                            //          
                            //      }
                            //  }
                            //      ));
                        }
                        else if (line [j] == '>')
                        {
                            var constraint = new Constraint(
                                    () =>
                                        (horizontalLines [row / 2] [col].Value ?? int.MaxValue)
                                        >
                                        (horizontalLines [row / 2 + 1] [col].Value ?? int.MinValue)
                                    );
                            horizontalLines[row / 2][col].Constraints.Add(constraint);
                            horizontalLines[row / 2 + 1][col].Constraints.Add(constraint);
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
                            var constraint = new Constraint(
                                    () =>
                                        (horizontalLines [row / 2] [col].Value ?? int.MinValue)
                                        <
                                        (horizontalLines [row / 2] [col + 1].Value ?? int.MaxValue)
                                    );
                            horizontalLines[row / 2][col].Constraints.Add(constraint);
                            horizontalLines[row / 2 ][col + 1].Constraints.Add(constraint);
                            j++;
                        }
                        else if (c == '>')
                        {
                            var constraint = 
                                new Constraint(
                                    () =>
                                        (horizontalLines [row / 2] [col].Value ?? int.MaxValue)
                                        >
                                        (horizontalLines [row / 2] [col + 1].Value ?? int.MinValue)
                                    );
                            horizontalLines[row / 2][col].Constraints.Add(constraint);
                            horizontalLines[row / 2 ][col + 1].Constraints.Add(constraint);
                            j++;
                        }

                    }
                }
            }
        }
    }

    public static class ExtensionMethodsFutoshiki
    {
        public static List<T> RemoveFromDomain<T>(this ILabel<T> label, Func<T?, bool> func) where T:struct
        {
            List<T> list = new();
            int max = label.FreeDomainValues.Count;
            
            for (int i = 0; i < max; i++)
            {
                if (func(label.FreeDomainValues[i]))
                {
                    list.Add(label.FreeDomainValues[i]);
                    label.FreeDomainValues.RemoveAt(i);
                    max--;
                }
            }
            //var result = label.FreeDomainValues.Where(v => func(v)).ToList();
            //label.FreeDomainValues.RemoveAll(v => func(v));
            return list;
        }
    }
}