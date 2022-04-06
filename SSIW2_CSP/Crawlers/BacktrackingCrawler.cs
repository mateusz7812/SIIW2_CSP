using System;
using System.Linq;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Labels;
using SSIW2_CSP.ValueSetters;

namespace SSIW2_CSP.Crawlers
{
    class BacktrackingCrawler<T> : IDomainCrawler<T> where T : struct
    {
        public bool HasNext { get; private set; } = true;
        private int _currentLabelIndex;
        private Problem<T> Problem { get; init; }
        public IValueSetter<T> ValueSetter { get; }

        public BacktrackingCrawler(Problem<T> problem, IValueSetter<T> valueSetter)
        {
            Problem = problem;
            ValueSetter = valueSetter;
        }

        public IConstraint Constraint { get; private set; }
        public ILabel<T> CurrentLabel { get; private set; }

        public void Initialize()
        {
            for (int i = 0; i < Problem.Labels.Count; i++)
            {
                Problem.Labels [i].RenewFreeDomainValues();
            }
            _currentLabelIndex = 0;
            Constraint = new Constraint(() =>
            {
                if (_currentLabelIndex == Problem.Labels.Count)
                {
                    _currentLabelIndex--;
                }
                return Problem.Labels[_currentLabelIndex].FreeDomainValues.Any();
            });
            CurrentLabel = Problem.Labels[0];
        }

        public void SetNext()
        {
            CurrentLabel = Problem.Labels [_currentLabelIndex];
            ValueSetter.SetNextFreeValue(CurrentLabel);
            if (_currentLabelIndex < Problem.Labels.Count)
            {
                _currentLabelIndex++;
            }
        }

        public void SetReturn()
        {
            if (_currentLabelIndex == Problem.Labels.Count)
            {
                return;
            }
            Problem.Labels [_currentLabelIndex].Value = null;
            Problem.Labels [_currentLabelIndex].RenewFreeDomainValues();
            if (_currentLabelIndex == 0)
            {
                HasNext = false;
            }
            else
            {
                _currentLabelIndex -= 1;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static void RenewFreeDomainValues<T>(this ILabel<T> label) where T:struct
        {
            label.FreeDomainValues.Clear();
            label.FreeDomainValues.AddRange(label.Domain.Values);
        }
    }
}