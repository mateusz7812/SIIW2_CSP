﻿namespace SSIW2_CSP
{
    class Problem<T> where T : struct
    {
        public int Dimension { get; }

        public Problem(int dimension, ProblemType type)
        {
            Dimension = dimension;
            Type = type;
        }

        ProblemType Type { get; }
    }
}
