using System;
using SSIW2_CSP.Controllers;
using SSIW2_CSP.Crawlers;
using SSIW2_CSP.DataLoaders;
using SSIW2_CSP.LabelOrderStrategies;
using SSIW2_CSP.ValueOrderStrategies;
using SSIW2_CSP.ValueSetters;

namespace SSIW2_CSP
{
    public static class ControllerBuilder
    {
        public static IController<int> Build(Problem<int> problem)
        {
            LoadData(problem);
            //var setter = new DefaultValueSetter<int>();
            var valueOrderStrategy = GetValueOrderStrategy(problem);
            var setter = new OrderedValueSetter<int>(valueOrderStrategy);
            var crawler = GetCrawler(problem, setter);
            problem.NotOrderedLabels = problem.Labels;
            OrderLabels(problem);
            return new Controller<int>(crawler, problem);
        }

        private static IValueOrderStrategy<int> GetValueOrderStrategy(Problem<int> problem)
        {
            return problem.ValueOrderStrategyType switch
            {
                ValueOrderStrategyType.Domain =>
                    new DomainValueOrderStrategy<int>(),
                ValueOrderStrategyType.Random =>
                    new RandomValueOrderStrategy<int>(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static void OrderLabels<T>(Problem<T> problem) where T:struct
        {
            problem.Labels = problem.LabelOrderStrategyType switch
            {
                LabelOrderStrategyType.Default => 
                    new DefaultLabelOrderStrategy<T>().OrderLabels(problem.NotOrderedLabels),
                LabelOrderStrategyType.MostConstrainedFirst => 
                    new MostConstrainedFirstLabelOrderStrategy<T>().OrderLabels(problem.NotOrderedLabels),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IDomainCrawler<int> GetCrawler(Problem<int> problem, IValueSetter<int> valueSetter)
        {
            return problem.CrawlerType switch
            {
                CrawlerType.Backtracking => new BacktrackingCrawler<int>(problem, valueSetter),
                CrawlerType.ForwardChecking => new ForwardCheckingCrawler<int>(problem, valueSetter),
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