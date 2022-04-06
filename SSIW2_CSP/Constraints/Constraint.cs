using System;

namespace SSIW2_CSP.Constraints
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
