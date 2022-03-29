using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class ListConstraint<T> : IConstraint where T : struct
    {
        private readonly List<ILabel<T>> labels;
        private readonly Func<IList<ILabel<T>>, bool> func;

        public ListConstraint(List<ILabel<T>> labels, Func<IList<ILabel<T>>, bool> func)
        {
            this.labels = labels;
            this.func = func;
        }

        public bool IsSatisfied()
        {
            return func.Invoke(labels);
        }
    }
}
