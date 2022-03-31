using System.Collections.Generic;

namespace SSIW2_CSP
{
    class Problem<T> where T : struct
    {
        public int Dimension { get; }

        public Problem(int dimension, ProblemType type)
        {
            Dimension = dimension;
            Type = type;
        }

        public List<IConstraint> Constraints { get; init; } = new ();
        public List<ILabel<T>> Labels { get; init; } = new ();
        public List<T? []> Solutions { get; init; } = new ();
        public ProblemType Type { get; }
    }
}
