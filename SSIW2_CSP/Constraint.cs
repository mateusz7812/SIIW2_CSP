using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class Constraint : IConstraint
    {
        private readonly Func<bool> func;

        public Constraint(Func<bool> func)
        {
            this.func = func;
        }

        public bool IsSatisfied()
        {
            return func.Invoke();
        }
    }
}
