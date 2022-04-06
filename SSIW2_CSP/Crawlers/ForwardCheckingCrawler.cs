﻿using System;
using System.Collections.Generic;
using System.Linq;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.Crawlers
{
    class ForwardCheckingCrawler<T> : IDomainCrawler<T> where T : struct
    {
        private List<LabelWithDeletedValues<T>> Labels;
        public bool HasNext { get; private set; } = true;
        private int _currentLabelIndex;
        private Problem<T> Problem { get; init; }
        public ForwardCheckingCrawler(Problem<T> problem)
        {
            Problem = problem;
        }

        public IConstraint Constraint { get; private set; }
        public ILabel<T> LastLabel { get; private set; }

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
            LastLabel = Problem.Labels[0];
        }


        public void SetNext()
        {
            LastLabel = Problem.Labels [_currentLabelIndex];
            LastLabel.SetNextFreeValue();
            //WriteValues();
            RemoveAssignedValueFromVariableDomain();
            RemoveValuesThatNotSatisfiesConstraints();
            //WriteFreeDomainValuesCount();
            if (_currentLabelIndex < Labels.Count)
            {
                _currentLabelIndex++;
            }
        }

        private void WriteFreeDomainValuesCount()
        {
            Console.WriteLine(string.Join("  ", Problem.Labels.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / Problem.Dimension)
                .Select(x => string.Join(" ", x.Select(l => l.Value.FreeDomainValues.Count)))) + "\t" + Problem.Solutions.Count);
        }

        private void WriteValues()
        {
            Console.WriteLine(string.Join("  ", Problem.Labels.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / Problem.Dimension)
                .Select(x => string.Join(" ", x.Select(l => l.Value.Value)))) + "\t" + Problem.Solutions.Count);
        }

        private void RemoveValuesThatNotSatisfiesConstraints()
        {
            foreach (var constraint in LastLabel.Constraints.OfType<ConstraintWithDomainChecking<T>>())
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
            
            //Console.WriteLine("return");
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
            //WriteValues();
            //WriteFreeDomainValuesCount();
            //if (Labels[_currentLabelIndex].FreeDomainValuesCount == 1)
            //{
            //    SetReturn();
            //}
        }
    }
}