using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SSIW2_CSP
{
    class Controller<T> : IController<T> where T : struct
    {
        private IDomainCrawler<T> Crawler { get; }
        private Problem<T> Problem { get; }

        public Controller(IDomainCrawler<T> crawler, Problem<T> problem)
        {
            Crawler = crawler;
            Problem = problem;
        }

        public void FindSolutions()
        {
            Crawler.InitializeLabels(Problem.Labels);
            while (Crawler.HasNext)
            {
                if (Problem.Constraints.All(c => c.IsSatisfied()))
                {
                    if (!Problem.Labels.Any(l => l.Value is null))
                    {
                        Problem.Solutions.Add(Problem.Labels.Select(l => l.Value).ToArray());
                    }
                }
                else
                {
                    Crawler.SetReturn();
                }
                Crawler.SetNext();
            }
        }
    }
}
