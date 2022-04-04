using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    public class Constraint : IConstraint
    {
        private readonly Func<bool> _isSatisfied;

        public Constraint(Func<bool> isSatisfied)
        {
            this._isSatisfied = isSatisfied;
        }

        public bool IsSatisfied()
        {
            return _isSatisfied.Invoke();
        }
    }
}
