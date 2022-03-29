using System;

namespace SSIW2_CSP
{
    class TwoLabelConstraint<T> : IConstraint where T : struct
    {
        private readonly ILabel<T> first;
        private readonly ILabel<T> second;
        private readonly Func<ILabel<T>, ILabel<T>, bool> func;

        public TwoLabelConstraint(in ILabel<T> first, in ILabel<T> second, in Func<ILabel<T>, ILabel<T>, bool> func)
        {
            this.first = first;
            this.second = second;
            this.func = func;
        }

        public bool IsSatisfied()
        {
            if ((first.Value is null) || (second.Value is null))
            {
                return true;
            }
            return func.Invoke(first, second);
        }
    }
}
