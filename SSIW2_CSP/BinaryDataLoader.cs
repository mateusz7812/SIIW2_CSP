using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SSIW2_CSP
{
    class BinaryDataLoader: IDataLoader
    {
        static string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private readonly string file_name;

        public BinaryDataLoader(int dimension)
        {
            file_name = $"{path}\\Data\\binary_{dimension}x{dimension}";
            Dimension = dimension;
        }

        public int Dimension { get; }

        public void LoadData(List<ILabel<int>> labels, List<IConstraint> constraints)
        {
            labels.Clear();
            constraints.Clear();
            List<List<ILabel<int>>> vertical_lines = new List<List<ILabel<int>>>(Dimension);
            List<List<ILabel<int>>> horizontal_lines = new List<List<ILabel<int>>>(Dimension);
            for (int i = 0; i < Dimension; i++)
            {
                vertical_lines.Add(new List<ILabel<int>>(Dimension));
                horizontal_lines.Add(new List<ILabel<int>>(Dimension));
            }
            string [] text = File.ReadAllText(file_name).Split("\r\n");
            for (int i = 0; i < text.Length; i++)
            {
                string line = text [i];
                char [] array = line.ToCharArray();
                for (int j = 0; j < array.Length; j++)
                {
                    char c = array [j];
                    IDomain<int> domain;
                    if (c == '1')
                    {
                        domain = new ConstDomain<int>(1);
                    }
                    else if (c == '0')
                    {
                        domain = new ConstDomain<int>(0);
                    }
                    else
                    {
                        domain = new SetDomain<int>() { 1, 0 };
                    }
                    ILabel<int> label = new Label<int>(domain);
                    labels.Add(label);
                    vertical_lines [j].Add(label);
                    horizontal_lines [i].Add(label);
                }
            }
            constraints.Add(
                new Constraint(
                    () => AllLinesDifferent(vertical_lines)));

            constraints.Add(
                new Constraint(
                    () => AllLinesDifferent(horizontal_lines)));
            constraints.AddRange(vertical_lines.Union(horizontal_lines)
                .SelectMany(line => new List<IConstraint>() {
                    new Constraint(
                        () => line.Count(l => l.Value == 1) <= (Dimension / 2) && line.Count(l => l.Value == 0) <= (Dimension / 2)),
                    new Constraint(
                        () => WithoutThreeSameNumbers(line)                        )
                }));
        }

        private bool AllLinesDifferent(List<List<ILabel<int>>> lines)
        {
            int start_index = 0;
            for (int i = start_index; i < Dimension; i++)
            {
                for (int j = i + 1; j < Dimension; j++)
                {
                    bool same = true;
                    for (int k = 0; k < Dimension; k++)
                    {
                        if (lines [i] [k] == null || lines [j] [k] == null)
                        {
                            same = false;
                        }
                        if (lines [i] [k] != lines [j] [k])
                        {
                            same = false;
                        }
                    }
                    if (same)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private static bool WithoutThreeSameNumbers(List<ILabel<int>> line)
        {
            int last = -1;
            int counter = 0;
            foreach (int? val in line.Select(l => l.Value))
            {
                if (val != null)
                {
                    if (last == val)
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                    if (counter == 2)
                    {
                        return false;
                    }
                    last = (int) val;
                }
                else
                {
                    last = -1;
                }
            }
            return true;
        }
    }
}

