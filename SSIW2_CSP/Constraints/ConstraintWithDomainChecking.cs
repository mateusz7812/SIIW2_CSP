using System;
using System.Collections.Generic;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.Constraints
{
    public class ConstraintWithDomainChecking<T>: Constraint where T:struct
    {
        public Func<ILabel<T>, Dictionary<int, List<T>>> RemoveWrongValuesFromDomain { get; private set; }

        public ConstraintWithDomainChecking(Func<bool> isSatisfied, Func<ILabel<T>, Dictionary<int, List<T>>> removeWrongValuesFromDomain) : base(isSatisfied)
        {
            RemoveWrongValuesFromDomain = removeWrongValuesFromDomain;
        }
    }
}
