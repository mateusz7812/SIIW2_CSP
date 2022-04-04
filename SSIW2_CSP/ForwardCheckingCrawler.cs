using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class ForwardCheckingCrawler<T> : IDomainCrawler<T> where T : struct
    {
        class LabelWithDeletedValues: ILabel<T>
        {
            public ILabel<T> Label { get; }
            public Dictionary<ILabel<T>, List<T>> RemovedValues = new();
            public LabelWithDeletedValues(ILabel<T> label)
            {
                Label = label;
            }

            public T? Value
            {
                get => Label.Value;
                set => Label.Value = value;
            }

            public IDomain<T> Domain => Label.Domain;
            public void SetNextFreeValue() => Label.SetNextFreeValue();

            public bool HasFreeValues => Label.HasFreeValues;
            public List<T> FreeDomainValues => Label.FreeDomainValues; 
            public int FreeDomainValuesCount => Label.FreeDomainValuesCount;
            public void RenewFreeDomainValues() => Label.RenewFreeDomainValues();

            public List<T> RemoveFromDomain(Func<T?, bool> func) => RemoveFromDomain(func);
        }

        private List<LabelWithDeletedValues> Labels;
        public bool HasNext { get; private set; } = true;
        private int _currentLabelIndex;
        private List<ConstraintWithDomainChecking<T>> Constraints { get; set; }
        private Problem<T> Problem { get; init; }
        public ForwardCheckingCrawler(Problem<T> problem)
        {
            Problem = problem;
        }

        public void Initialize()
        {
            Labels = new();
            Constraints = new();
            Constraints.AddRange(Problem.Constraints.OfType<ConstraintWithDomainChecking<T>>());
            for (int i = 0; i < Problem.Labels.Count; i++)
            {
                Problem.Labels [i].RenewFreeDomainValues();
                Labels.Add(new LabelWithDeletedValues(Problem.Labels [i]));
            }
            _currentLabelIndex = 0;
        }

        public void SetNext()
        {
            if (_currentLabelIndex == Labels.Count)
            {
                _currentLabelIndex--;
            }
            
            while (Labels.Skip(_currentLabelIndex).Any(l => !l.HasFreeValues))
            {
                SetReturn();
            }
            Labels[_currentLabelIndex].SetNextFreeValue();
            WriteValues();
            RemoveAssignedValueFromVariableDomain();
            RemoveValuesThatNotSatisfiesConstraints();
            WriteFreeDomainValuesCount();
            if (_currentLabelIndex < Labels.Count)
            {
                _currentLabelIndex++;
            }
        }

        private void WriteFreeDomainValuesCount()
        {
            Console.WriteLine(string.Join("  ", Problem.Labels.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / Problem.Dimension)
                .Select(x => string.Join(" ", x.Select(l => l.Value.FreeDomainValuesCount)))) + "\t" + Problem.Solutions.Count);
        }

        private void WriteValues()
        {
            Console.WriteLine(string.Join("  ", Problem.Labels.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / Problem.Dimension)
                .Select(x => string.Join(" ", x.Select(l => l.Value.Value)))) + "\t" + Problem.Solutions.Count);
        }

        private void RemoveValuesThatNotSatisfiesConstraints()
        {
            foreach (var constraint in Constraints)
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
            if (!Labels[_currentLabelIndex].RemovedValues.ContainsKey(Labels[_currentLabelIndex].Label))
            {
                Labels[_currentLabelIndex].RemovedValues.Add(Labels[_currentLabelIndex].Label, new List<T>());
            }

            Labels[_currentLabelIndex].RemovedValues[Labels[_currentLabelIndex].Label]
                .Add((T) Labels[_currentLabelIndex].Value);
        }

        public void SetReturn()
        {
            if (_currentLabelIndex == Problem.Labels.Count)
            {
                return;
            }
            
            Console.WriteLine("return");
            Labels [_currentLabelIndex].Value = null;
            if (Labels[_currentLabelIndex].RemovedValues.ContainsKey(Labels[_currentLabelIndex].Label))
            {
                Labels[_currentLabelIndex].FreeDomainValues.AddRange(Labels[_currentLabelIndex].RemovedValues[Labels[_currentLabelIndex].Label]);
                Labels[_currentLabelIndex].RemovedValues[Labels[_currentLabelIndex].Label].Clear();
            } 
            
            if (_currentLabelIndex == 0)
            {
                HasNext = false;
            }
            else
            {
                _currentLabelIndex -= 1;
            }
            
            foreach (var label in Labels[_currentLabelIndex].RemovedValues.Keys)
            {
                if(label == Labels[_currentLabelIndex].Label) continue;
                label.FreeDomainValues.AddRange(Labels[_currentLabelIndex].RemovedValues[label]);
                Labels[_currentLabelIndex].RemovedValues[label].Clear();
            }
            WriteValues();
            WriteFreeDomainValuesCount();
            //if (Labels[_currentLabelIndex].FreeDomainValuesCount == 1)
            //{
            //    SetReturn();
            //}
        }
    }
}