using System.Linq;
using SSIW2_CSP.Crawlers;

namespace SSIW2_CSP.Controllers
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
                if (Crawler.LastLabel.Constraints.All(c => c.IsSatisfied()) 
                    && Problem.Constraints.All(c => c.IsSatisfied()))
                {
                    if (!Problem.Labels.Any(l => l.Value is null))
                    {
                        Problem.Solutions.Add(Problem.NotOrderedLabels.Select(l => l.Value).ToArray());
                    }
                }
                else
                {
                    Crawler.SetReturn();
                }

                while (!Crawler.Constraint.IsSatisfied())
                {
                    Crawler.SetReturn();
                }
                
                Crawler.SetNext();
                StepsCounter++;
            }
        }
    }
}
