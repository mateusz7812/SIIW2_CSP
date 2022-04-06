using System;
using System.Linq;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.Crawlers
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

        public IConstraint Constraint { get; private set; }
        public ILabel<T> LastLabel { get; private set; }

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
            LastLabel = Problem.Labels[0];
        }

        public void SetNext()
        {
            LastLabel = Problem.Labels [_currentLabelIndex];
            LastLabel.SetNextFreeValue();
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

    public static class ExtensionMethods
    {
        public static void RenewFreeDomainValues<T>(this ILabel<T> label) where T:struct
        {
            label.FreeDomainValues.Clear();
            label.FreeDomainValues.AddRange(label.Domain.Values);
        }
        
        public static void SetNextFreeValue<T>(this ILabel<T> label) where T: struct
        {
            label.Value = label.FreeDomainValues.First();
            label.FreeDomainValues.Remove((T) label.Value);
        }
    }
}