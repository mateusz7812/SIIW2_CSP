using System;
using System.Collections.Generic;
using System.Linq;
using SSIW2_CSP.Controllers;
using SSIW2_CSP.Crawlers;
using SSIW2_CSP.DataLoaders;
using SSIW2_CSP.LabelOrderStrategies;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP
{
    class Program
    {

        static void Main(string [] args)
        {
            Problem<int> problem = new Problem<int>(6, ProblemType.Futoshiki, CrawlerType.Backtracking, LabelOrderStrategyType.MostConstrainedFirst);
            LoadData(problem);
            var crawler = GetCrawler(problem);
            problem.NotOrderedLabels = problem.Labels;
            OrderLabels(problem);
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

        private static void OrderLabels<T>(Problem<T> problem) where T:struct
        {
            switch (problem.LabelOrderStrategyType)
            {
                case LabelOrderStrategyType.Default:
                    problem.Labels = new DefaultLabelOrderStrategy<T>().OrderLabels(problem.NotOrderedLabels);
                    break;
                case LabelOrderStrategyType.MostConstrainedFirst:
                    problem.Labels = new MostConstrainedFirstLabelOrderStrategy<T>().OrderLabels(problem.NotOrderedLabels);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IDomainCrawler<int> GetCrawler(Problem<int> problem)
        {
            return problem.CrawlerType switch
            {
                CrawlerType.Backtracking => new BacktrackingCrawler<int>(problem),
                CrawlerType.ForwardChecking => new ForwardCheckingCrawler<int>(problem),
                _ => throw new NotImplementedException()
            };
        }

        private static void LoadData(Problem<int> problem)
        {
            switch (problem.ProblemType)
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
        }
    }
}
