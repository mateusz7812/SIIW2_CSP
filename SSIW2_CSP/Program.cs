using System;
using System.Collections.Generic;

namespace SSIW2_CSP
{
    class Program
    {
        private static List<ListConstraint<int>> constraints;

        static void Main(string[] args)
        {
            List<Label<int>> labels;
            new BinaryDataLoader(6).LoadData(out labels, out constraints);
        }
    }
}
