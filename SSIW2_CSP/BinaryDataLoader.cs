using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SSIW2_CSP
{
    class BinaryDataLoader
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
            List<ILabel<int>> vertical_line = vertical_lines [0];
            //constraints.Add(
            //        new ListConstraint<int>(
            //            ref labels,
            //            labels =>
            //            {
            //                return labels.Count(l => l.Value == 1) < (Dimension / 2) && labels.Count(l => l.Value == 0) < (Dimension / 2);
            //            }));
            //constraints.Add(
            //        new ListConstraint<int>(
            //            ref vertical_line,
            //            labels =>
            //            {
            //                return labels.Count(l => l.Value == 1) < (Dimension / 2) && labels.Count(l => l.Value == 0) < (Dimension / 2);
            //            }));
            constraints.AddRange(vertical_lines.Union(horizontal_lines)
                .SelectMany(line => new List<ListConstraint<int>>() {
                    new ListConstraint<int>(
                        line,
                        labels => {
                            //Console.WriteLine("test => " + string.Join(" ", labels.Select(l => l.Value)));
                            return labels.Count(l => l.Value == 1) <= (Dimension / 2) && labels.Count(l => l.Value == 0) <= (Dimension / 2);
                        }),
                    new ListConstraint<int>(
                        line,
                        labels =>
                        {
                            //Console.WriteLine("test => " + string.Join(" ", labels.Select(l => l.Value)));
                            int last = -1;
                            int counter = 0;
                            foreach(int? val in labels.Select(l => l.Value))
                            {
                                if(val != null)
                                {
                                    if(last == val)
                                    {
                                        counter ++;
                                    } else
                                    {
                                        counter = 0;
                                    }
                                    if(counter == 2)
                                    {
                                        //Console.WriteLine("test => " + string.Join(" ", labels.Select(l => l.Value)));
                                        return false;
                                    }
                                    last = (int)val;
                                } else
                                {
                                    last = -1;
                                }
                            }
                            return true;
                        }
                        )
                    //,
                    //new ListConstraint<int>(
                    //    labels,
                    //    labels => !labels.Any(l => l.Value is null)
                    //    )
                })); ;
        }
    }
}

