using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class BacktrackingCrawler<T> : IDomainCrawler<T> where T : struct
    {
        class LabelDecorator<T>: ILabel<T> where T : struct
        {
            public List<T> FreeDomainValues { get; init; } = new List<T>();
            private ILabel<T> Label { get; init; }
            public T? Value { 
                get => Label.Value; 
                set => Label.Value = value; 
            }

            public IDomain<T> Domain => Label.Domain;

            public LabelDecorator(ILabel<T> label)
            {
                Label = label;
            }

            internal void SetNextFreeValue()
            {
                Label.Value = FreeDomainValues.First();
                FreeDomainValues.Remove((T) Value);
            }

            internal bool HasFreeValues() => FreeDomainValues.Any();

            internal void RenewFreeDomainValues()
            {
                FreeDomainValues.Clear();
                FreeDomainValues.AddRange(Domain.Values);
            }
        }

        private List<LabelDecorator<T>> Labels { get; set; } = new List<LabelDecorator<T>>();
        public bool HasNext { get; private set; } = true;
        private int currentLabelIndex = 0;

        public void InitializeLabels(List<ILabel<T>> labels)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                Labels.Add(new LabelDecorator<T>(labels [i]));
                Labels [i].RenewFreeDomainValues();
                labels [i] = Labels [i];
            }
            currentLabelIndex = 0;
        }

        public void SetNext()
        {
            while (!Labels [currentLabelIndex].HasFreeValues())
            {
                SetReturn();
            }
            Labels [currentLabelIndex].SetNextFreeValue();
            if (currentLabelIndex < Labels.Count - 1)
            {
                currentLabelIndex += 1;
            }
            Console.WriteLine(string.Join("  ", Labels.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 6)
                .Select(x => string.Join(" ", x.Select(l => l.Value.FreeDomainValues.Count)))));
        }

        public void SetReturn()
        {
            Labels [currentLabelIndex].Value = null;
            Labels [currentLabelIndex].RenewFreeDomainValues();
            if (currentLabelIndex == 0)
            {
                HasNext = false;
            }
            else
            {
                currentLabelIndex -= 1;
            }
        }
    }


}
