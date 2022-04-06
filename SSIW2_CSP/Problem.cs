using System.Collections.Generic;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.LabelOrderStrategies;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP
{
    class Problem<T> where T : struct
    {
        public int Dimension { get; }

        public Problem(int dimension, ProblemType problemType, CrawlerType crawlerType,
            LabelOrderStrategyType labelOrderStrategyType)
        {
            Dimension = dimension;
            ProblemType = problemType;
            CrawlerType = crawlerType;
            LabelOrderStrategyType = labelOrderStrategyType;
        }

        public List<IConstraint> Constraints { get; init; } = new ();
        public List<ILabel<T>> Labels { get; set; } = new ();
        public List<T? []> Solutions { get; init; } = new ();
        public ProblemType ProblemType { get; }
        public CrawlerType CrawlerType { get; }
        public LabelOrderStrategyType LabelOrderStrategyType { get; }
        public List<ILabel<T>> NotOrderedLabels { get; set; }
    }
}
