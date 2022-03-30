using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class Program
    {

        static void Main(string [] args)
        {
            const int dimension = 6;
            const ProblemType problemType = ProblemType.FUTOSHIKI;
            Problem<int> problem = new Problem<int>(dimension, problemType);
            List<ILabel<int>> labels = new();
            List <IConstraint> constraints = new();
            switch (problemType)
            {
                case ProblemType.BINARY:
                    new BinaryDataLoader(problem.Dimension).LoadData(labels, constraints);
                    break;
                case ProblemType.FUTOSHIKI:
                    new FutoshikiDataLoader(problem.Dimension).LoadData(labels, constraints);
                    break;
                default:
                    throw new NotImplementedException();
            }

            BacktrackingCrawler<int> crawler = new BacktrackingCrawler<int>(problem);
            Controller<int> controller = new Controller<int>(crawler, labels, constraints);
            int [] [] solutions = controller.GetSolutions();
            foreach(int[] solution in solutions)
            {
                Console.WriteLine("\n" + string.Join("\n", solution.Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / problem.Dimension)
                    .Select(x => string.Join(" ", x.Select(l => l.Value)))));
            }
        }
    }
}
