using System;
using System.Collections.Generic;
using System.Linq;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Labels;
using SSIW2_CSP.ValueSetters;

namespace SSIW2_CSP.Crawlers
{
    class ForwardCheckingCrawler<T> : IDomainCrawler<T> where T : struct
    {
        private List<LabelWithDeletedValues<T>> Labels;
        public bool HasNext { get; private set; } = true;
        private int _currentLabelIndex;
        private Problem<T> Problem { get; init; }
        public IValueSetter<T> ValueSetter { get; }

        public ForwardCheckingCrawler(Problem<T> problem, IValueSetter<T> valueSetter)
        {
            Problem = problem;
            ValueSetter = valueSetter;
        }

        public IConstraint Constraint { get; private set; }
        public ILabel<T> CurrentLabel { get; private set; }

        public void Initialize()
        {
            Labels = new();
            for (int i = 0; i < Problem.Labels.Count; i++)
            {
                Problem.Labels [i].RenewFreeDomainValues();
                Labels.Add(new LabelWithDeletedValues<T>(Problem.Labels [i], i));
            }
            _currentLabelIndex = 0;
            Constraint =
                new Constraint(() =>
                {
                    if (_currentLabelIndex == Labels.Count)
                    {
                        _currentLabelIndex--;
                    }
                    return Labels.Skip(_currentLabelIndex).All(l => l.FreeDomainValues.Any<T>());
                });
            CurrentLabel = Problem.Labels[0];
        }


        public void SetNext()
        {
            CurrentLabel = Problem.Labels [_currentLabelIndex];
            ValueSetter.SetNextFreeValue(CurrentLabel);
            RemoveAssignedValueFromVariableDomain();
            RemoveValuesThatNotSatisfiesConstraints();
            if (_currentLabelIndex < Labels.Count)
            {
                _currentLabelIndex++;
            }
        }
        
        private void RemoveValuesThatNotSatisfiesConstraints()
        {
            foreach (var constraint in CurrentLabel.Constraints.OfType<ConstraintWithDomainChecking<T>>())
            {
                var dict = constraint.RemoveWrongValuesFromDomain.Invoke(Labels[_currentLabelIndex].Label);
                foreach (var pair in dict)
                {
                    if (!Labels[_currentLabelIndex].RemovedValues.ContainsKey(pair.Key))
                    {
                        Labels[_currentLabelIndex].RemovedValues.Add(pair.Key, new List<T>());
                    }

                    Labels[_currentLabelIndex].RemovedValues[pair.Key].AddRange(pair.Value);
                }
            }
        }

        private void RemoveAssignedValueFromVariableDomain()
        {
            if (!Labels[_currentLabelIndex].RemovedValues.ContainsKey(Labels[_currentLabelIndex].ID))
            {
                Labels[_currentLabelIndex].RemovedValues.Add(Labels[_currentLabelIndex].ID, new List<T>());
            }

            Labels[_currentLabelIndex].RemovedValues[Labels[_currentLabelIndex].ID]
                .Add((T) Labels[_currentLabelIndex].Value);
        }

        public void SetReturn()
        {
            if (_currentLabelIndex == Problem.Labels.Count)
            {
                return;
            }
            
            Labels [_currentLabelIndex].Value = null;
            if (Labels[_currentLabelIndex].RemovedValues.ContainsKey(Labels[_currentLabelIndex].ID))
            {
                Labels[_currentLabelIndex].FreeDomainValues.AddRange(Labels[_currentLabelIndex].RemovedValues[Labels[_currentLabelIndex].ID]);
                Labels[_currentLabelIndex].RemovedValues[Labels[_currentLabelIndex].ID].Clear();
            } 
            
            if (_currentLabelIndex == 0)
            {
                HasNext = false;
            }
            else
            {
                _currentLabelIndex -= 1;
            }
            
            for(int i = 0; i < Labels.Count;i++)
                if (_currentLabelIndex != i && Labels[_currentLabelIndex].RemovedValues.ContainsKey(i))
                {
                    Labels[i].FreeDomainValues.AddRange(Labels[_currentLabelIndex].RemovedValues[i]);
                    Labels[_currentLabelIndex].RemovedValues[i].Clear();
                }
        }
    }
}