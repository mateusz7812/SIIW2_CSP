using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class Constraint : IConstraint
    {
        private readonly Func<bool> _func;

        public Constraint(Func<bool> func)
        {
            this._func = func;
        }

        public bool IsSatisfied()
        {
            return _func.Invoke();
        }
    }
}
