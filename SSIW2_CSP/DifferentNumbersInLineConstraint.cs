using System.Collections.Generic;

namespace SSIW2_CSP
{
    class DifferentNumbersInLineConstraint : IConstraint
    {
        private static List<int?> _numbers;
        private readonly int _dimension;
        private readonly List<ILabel<int>> _line;

        public DifferentNumbersInLineConstraint(int dimension, List<ILabel<int>> line)
        {
            _dimension = dimension;
            _line = line;
            _numbers = new List<int?>(dimension);
        }

        public bool IsSatisfied()
        {
            _numbers.Clear();
            for (int i = 0; i < _dimension; i++)
            {
                if (_line[i].Value == null) continue;
                if (_numbers.Contains(_line [i].Value))
                {
                    return false;
                }

                _numbers.Add(_line [i].Value);

            }
            return true;
        }
    }
}