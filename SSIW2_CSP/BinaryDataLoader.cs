using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void LoadData(out List<Label<int>> labels, out List<ListConstraint<int>> constraints)
        {
            labels = new();
            constraints = new();
            Label<int>[][] vertical_lines = new Label<int>[Dimension][];
            Label<int>[][] horizontal_lines = new Label<int>[Dimension][];
            for(int i = 0; i < Dimension; i++)
            {
                vertical_lines [i] = new Label<int>[Dimension];
                horizontal_lines [i] = new Label<int>[Dimension];
            }
            string[] text = File.ReadAllText(file_name).Split("\r\n");
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
                    } else if (c == '0')
                    {
                        domain = new ConstDomain<int>(0);
                    } else
                    {
                        domain = new SetDomain<int>() { 1, 0 };
                    }
                    Label<int> label = new Label<int>(domain);
                    labels.Add(label);
                    vertical_lines [j] [i] = label;
                    horizontal_lines [i] [j] = label;
                }
            }
            constraints = vertical_lines.Union(horizontal_lines)
                .Select(line => 
                    new ListConstraint<int>(
                        line, 
                        labels => 
                            labels.Count(l => l.Value == 1) == labels.Count(l => l.Value == 0)))
                .ToList();
        }
    }
}

