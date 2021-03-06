using System;
using System.Collections.Generic;
using System.Linq;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Crawlers;
using SSIW2_CSP.DataLoaders;
using SSIW2_CSP.LabelOrderStrategies;
using SSIW2_CSP.Labels;
using SSIW2_CSP.ValueOrderStrategies;

namespace SSIW2_CSP
{
    public class Problem<T> where T : struct
    {
        public int Dimension { get; }

        public Problem(int dimension, ProblemType problemType, CrawlerType crawlerType,
            LabelOrderStrategyType labelOrderStrategyType, ValueOrderStrategyType valueOrderStrategyType,
            bool debug)
        {
            Dimension = dimension;
            ProblemType = problemType;
            CrawlerType = crawlerType;
            LabelOrderStrategyType = labelOrderStrategyType;
            ValueOrderStrategyType = valueOrderStrategyType;
            Debug = debug;
        }

        public List<IConstraint> Constraints { get; init; } = new ();
        public List<ILabel<T>> Labels { get; set; } = new ();
        public List<Solution<T>> Solutions { get; init; } = new ();
        public ProblemType ProblemType { get; }
        public CrawlerType CrawlerType { get; }
        public LabelOrderStrategyType LabelOrderStrategyType { get; }
        public List<ILabel<T>> NotOrderedLabels { get; set; }
        public ValueOrderStrategyType ValueOrderStrategyType { get; }
        public bool Debug { get; }

        public void WriteFreeDomainValuesCount()
        {
            if(Debug)
                Console.WriteLine(string.Join("  ", Labels.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / Dimension)
                .Select(x => string.Join(" ", x.Select(l => l.Value.FreeDomainValues.Count)))));
        }
        
        public void WriteValues()
        {
            if(Debug)
                Console.WriteLine(string.Join("  ", Labels.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / Dimension)
                .Select(x => string.Join(" ", x.Select(l => l.Value.Value.HasValue ? l.Value.Value.ToString() : "-")))));
        }
    }
}
