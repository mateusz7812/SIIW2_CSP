using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    class TwoLabelConstraint<T> : IConstraint where T: struct
    {
        private readonly Label<T> first;
        private readonly Label<T> second;
        private readonly Func<Label<T>, Label<T>, bool> func;

        public TwoLabelConstraint(in Label<T> first, in Label<T> second, in Func<Label<T>, Label<T>, bool> func)
        {
            this.first = first;
            this.second = second;
            this.func = func;
        }
        
        public bool IsSatisfied()
        {
            if((first.Value is null) || (second.Value is null))
            {
                return true;
            }
            return func.Invoke(first, second);
        }
    }
}
