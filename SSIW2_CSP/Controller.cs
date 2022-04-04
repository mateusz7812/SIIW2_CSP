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
        public int StepsCounter { get; set; }
        public Controller(IDomainCrawler<T> crawler, Problem<T> problem)
        {
            Crawler = crawler;
            Problem = problem;
        }

        public void FindSolutions()
        {
            StepsCounter = 0;
            Crawler.Initialize();
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
                StepsCounter++;
            }
        }
    }
}
