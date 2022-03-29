using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class Program
    {

        static void Main(string [] args)
        {
            List<ILabel<int>> labels = new();
            List <IConstraint> constraints = new();
            new BinaryDataLoader(6).LoadData(labels, constraints);
            BacktrackingCrawler<int> crawler = new BacktrackingCrawler<int>();
            AbstractController<int> controller = new AbstractController<int>(crawler, labels, constraints);
            int [] [] solutions = controller.GetSolutions();
            foreach(int[] solution in solutions)
            {
                Console.WriteLine(string.Join("\n", solution.Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / 6)
                    .Select(x => string.Join(" ", x.Select(l => l.Value)))));
            }
        }
    }
}
