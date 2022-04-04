using System;
using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP
{
    class BacktrackingCrawler<T> : IDomainCrawler<T> where T : struct
    {
        public bool HasNext { get; private set; } = true;
        private int _currentLabelIndex;
        private Problem<T> Problem { get; init; }
        public BacktrackingCrawler(Problem<T> problem)
        {
            Problem = problem;
        }

        public void Initialize()
        {
            for (int i = 0; i < Problem.Labels.Count; i++)
            {
                Problem.Labels [i].RenewFreeDomainValues();
            }
            _currentLabelIndex = 0;
        }

        public void SetNext()
        {
            if (_currentLabelIndex == Problem.Labels.Count)
            {
                _currentLabelIndex--;
            }
            while (!Problem.Labels [_currentLabelIndex].HasFreeValues)
            {
                SetReturn();
            }
            Problem.Labels [_currentLabelIndex].SetNextFreeValue();
            if (_currentLabelIndex < Problem.Labels.Count)
            {
                _currentLabelIndex++;
            }
            //Console.WriteLine(string.Join("  ", Labels.Select((x, i) => new { Index = i, Value = x })
            //    .GroupBy(x => x.Index / Problem.Dimension)
            //    .Select(x => string.Join(" ", x.Select(l => l.Value.FreeDomainValues.Count)))) + "\t" + Problem.Solutions.Count);
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
}