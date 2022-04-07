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
                Labels.Add((LabelWithDeletedValues<T>) Problem.Labels [i]);
                Labels[i].ID = i;
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
            CurrentLabel = Labels[0];
        }


        public void SetNext()
        {
            CurrentLabel = Labels [_currentLabelIndex];
            ValueSetter.SetNextFreeValue(CurrentLabel);
            RemoveAssignedValueFromVariableDomain();
            RemoveValuesThatNotSatisfiesConstraints();
            Problem.WriteValues();
            Problem.WriteFreeDomainValuesCount();
            WriteDeletedDomainValueCount();
            //Console.WriteLine();
            if (_currentLabelIndex < Labels.Count)
            {
                _currentLabelIndex++;
            }
        }
        
        private void RemoveValuesThatNotSatisfiesConstraints()
        {
            foreach (var constraint in CurrentLabel.Constraints.OfType<ConstraintWithDomainChecking<T>>())
            {
                Dictionary<int,List<T>> dict = constraint.RemoveWrongValuesFromDomain.Invoke(Labels[_currentLabelIndex]);
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
            var removedValues = Labels[_currentLabelIndex].RemovedValues;
            if (!removedValues.ContainsKey(Labels[_currentLabelIndex].ID))
            {
                removedValues.Add(Labels[_currentLabelIndex].ID, new List<T>());
            }

            removedValues[Labels[_currentLabelIndex].ID]
                .Add((T) Labels[_currentLabelIndex].Value);
        }

        public void SetReturn()
        {
            if (_currentLabelIndex == Labels.Count)
            {
                return;
            }
            
            //Console.WriteLine("return");
            //Problem.WriteValues();
            
            Labels [_currentLabelIndex].Value = null;
            
            //Problem.WriteValues();
            Problem.WriteFreeDomainValuesCount();
            WriteDeletedDomainValueCount();
            
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
            Problem.WriteFreeDomainValuesCount();
            WriteDeletedDomainValueCount();

            for(int i = 0; i < Labels.Count;i++)
            {
                if (_currentLabelIndex != i && Labels[_currentLabelIndex].RemovedValues.ContainsKey(i))
                {
                    var freeDomainValues = Labels[i].FreeDomainValues;
                    var removedValue = Labels[_currentLabelIndex].RemovedValues[i];
                    freeDomainValues.AddRange(removedValue);
                    removedValue.Clear();
                }
            }
            
            //Problem.WriteFreeDomainValuesCount();
            Problem.WriteFreeDomainValuesCount();
            WriteDeletedDomainValueCount();
        }

        private void WriteDeletedDomainValueCount()
        {
            if(Problem.Debug)
                Console.WriteLine(string.Join("  ", Labels.Select((x, i) => new {Index = i, Value = x})
                    .GroupBy(x => x.Index / Problem.Dimension)
                    .Select(x => string.Join(" ", x.Select(l => l.Value.RemovedValues.Values.Sum(v => v.Count))))));
        }
    }
}