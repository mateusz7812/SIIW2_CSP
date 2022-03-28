using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    class ListConstraint<T> : IConstraint where T: struct 
    {
        private readonly Label<T>[] labels;
        private readonly Func<Label<T> [], bool> func;

        public ListConstraint(Label<T> [] labels, Func<Label<T> [], bool> func)
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
