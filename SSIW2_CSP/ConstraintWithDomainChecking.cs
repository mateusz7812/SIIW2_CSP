using System;
using System.Collections.Generic;

namespace SSIW2_CSP
{
    public class ConstraintWithDomainChecking<T>: Constraint where T:struct
    {
        public Func<ILabel<T>, Dictionary<ILabel<T>, List<T>>> RemoveWrongValuesFromDomain { get; private set; }

        public ConstraintWithDomainChecking(Func<bool> isSatisfied, Func<ILabel<T>, Dictionary<ILabel<T>, List<T>>> removeWrongValuesFromDomain) : base(isSatisfied)
        {
            RemoveWrongValuesFromDomain = removeWrongValuesFromDomain;
        }
    }
}
