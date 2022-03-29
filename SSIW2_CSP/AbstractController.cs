using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SSIW2_CSP
{
    class AbstractController<T> : IController<T> where T : struct
    {
        private IDomainCrawler<T> crawler { get; set; }

        public AbstractController(IDomainCrawler<T> crawler, List<ILabel<T>> labels, List<IConstraint> constraints)
        {
            this.crawler = crawler;
            Labels = labels;
            Constraints = constraints;
        }

        private List<T []> Solutions { get; set; } = new List<T []>();
        private List<ILabel<T>> Labels { get; set; }
        private List<IConstraint> Constraints { get; set; }

        public T [] [] GetSolutions()
        {
            crawler.InitializeLabels(Labels);
            while (crawler.HasNext)
            {
                if (!Constraints.Any(c => !c.IsSatisfied()))
                {
                    if (!Labels.Any(l => l.Value is null))
                    {
                        Solutions.Add(Labels.Select(l => (T) l.Value).ToArray());
                    }
                }
                else
                {
                    crawler.SetReturn();
                }
                crawler.SetNext();
                //Console.WriteLine(string.Join(" ", Labels.Select(l => l.Value)));
                //Thread.Sleep(500);
            }
            return Solutions.ToArray();
        }


    }
}
