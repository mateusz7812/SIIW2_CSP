using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class Program
    {

        static void Main(string [] args)
        {
            const int dimension = 4;
            const ProblemType problemType = ProblemType.Futoshiki;
            Problem<int> problem = new Problem<int>(dimension, problemType);
            switch (problemType)
            {
                case ProblemType.Binary:
                    new BinaryDataLoader(problem.Dimension).LoadData(problem.Labels, problem.Constraints);
                    break;
                case ProblemType.Futoshiki:
                    new FutoshikiDataLoader(problem.Dimension).LoadData(problem.Labels, problem.Constraints);
                    break;
                default:
                    throw new NotImplementedException();
            }

            //BacktrackingCrawler<int> crawler = new BacktrackingCrawler<int>(problem);
            ForwardCheckingCrawler<int> crawler = new ForwardCheckingCrawler<int>(problem);
            Controller<int> controller = new Controller<int>(crawler, problem);
            controller.FindSolutions();
            foreach(int?[] solution in problem.Solutions)
            {
                Console.WriteLine("\n" + string.Join("\n", solution.Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / problem.Dimension)
                    .Select(x => string.Join(" ", x.Select(l => l.Value)))));
            }
            Console.WriteLine($"Found {problem.Solutions.Count} solutions");
            Console.WriteLine($"Taking {controller.StepsCounter} steps");
        }
    }
}
