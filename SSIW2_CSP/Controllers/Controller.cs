using System;
using System.Diagnostics;
using System.Linq;
using SSIW2_CSP.Crawlers;

namespace SSIW2_CSP.Controllers
{
    class Controller<T> : IController<T> where T : struct
    {
        private IDomainCrawler<T> Crawler { get; }
        private Problem<T> Problem { get; }

        public int ReturnsCounter { get; set; }
        public int LastReturnsCounter { get; set; }

        public Stopwatch Stopwatch { get; } = new Stopwatch();
        public long LastMillisecondsCounter { get; set; }
        private int StepsCounter { get; set; }
        private int LastStepsCounter { get; set; }
        public Controller(IDomainCrawler<T> crawler, Problem<T> problem)
        {
            Crawler = crawler;
            Problem = problem;
        }

        public void FindSolutions()
        {
            StepsCounter = 0;
            LastStepsCounter = 0;
            ReturnsCounter = 0;
            LastReturnsCounter = 0;
            LastReturnsCounter = 0;
            Stopwatch.Start();
            Crawler.Initialize();
            while (Crawler.HasNext)
            {
                if (Crawler.CurrentLabel.Constraints.All(c => c.IsSatisfied()) 
                    && Problem.Constraints.All(c => c.IsSatisfied()))
                {
                    if (!Problem.Labels.Any(l => l.Value is null))
                    {
                        Problem.Solutions.Add(
                            new Solution<T>(
                                    Problem.NotOrderedLabels.Select(l => l.Value).ToArray(), 
                                    StepsCounter, 
                                    StepsCounter - LastStepsCounter, 
                                    ReturnsCounter, 
                                    ReturnsCounter - LastReturnsCounter,
                                    Stopwatch.ElapsedMilliseconds,
                                    Stopwatch.ElapsedMilliseconds - LastMillisecondsCounter));
                        LastStepsCounter = StepsCounter;
                        LastReturnsCounter = ReturnsCounter;
                        LastMillisecondsCounter = Stopwatch.ElapsedMilliseconds;
                    }
                }
                else
                {
                    Crawler.SetReturn();
                    ReturnsCounter++;
                }

                while (!Crawler.Constraint.IsSatisfied())
                {
                    Crawler.SetReturn();
                    ReturnsCounter++;
                }
                
                Crawler.SetNext();
                StepsCounter++;
            }
            Stopwatch.Stop();
        }


        public void PrintResult()
        {
            for (var index = 0; index < Problem.Solutions.Count; index++)
            {
                var solution = Problem.Solutions[index];
                Console.WriteLine("{0, 4} {1, 5} {2, 8} {3, 8} {4, 10} {5, 10} {6, 10}",
                    index + 1, 
                    solution.MillisecondsFromLast, solution.StepsCounterFromLast, solution.ReturnsCounterFromLast, 
                    solution.CurrentMilliseconds, solution.CurrentStepsNumber, solution.CurrentReturnsNumber);
                //Console.WriteLine(string.Join("\n", solution.Labels.Select((x, i) => new { Index = i, Value = x })
                //    .GroupBy(x => x.Index / Problem.Dimension)
                //    .Select(x => string.Join(" ", x.Select(l => l.Value)))));
            }

            Console.WriteLine();
            Console.WriteLine($"ProblemType \t\t{Problem.ProblemType}");
            Console.WriteLine($"CrawlerType \t\t{Problem.CrawlerType}");
            Console.WriteLine($"LabelOrderStrategyType \t{Problem.LabelOrderStrategyType}");
            Console.WriteLine($"ValueOrderStrategyType \t{Problem.ValueOrderStrategyType}");
            Console.WriteLine($"Found solutions \t{Problem.Solutions.Count}");
            Console.WriteLine($"Taking steps \t\t{StepsCounter} ");
            Console.WriteLine($"Taking returns \t\t{ReturnsCounter} ");
            Console.WriteLine($"Elapsed milliseconds \t\t{Stopwatch.ElapsedMilliseconds} ");
        }
    }
}
