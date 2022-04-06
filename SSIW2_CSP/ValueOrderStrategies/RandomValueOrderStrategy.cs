using System;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.ValueOrderStrategies
{
    public class RandomValueOrderStrategy<T>: IValueOrderStrategy<T> where T:struct
    {
        private readonly Random _random = new Random();
        public T? GetNext(ILabel<T> label)
        {
            return label.FreeDomainValues[_random.Next(label.FreeDomainValues.Count)];
        }
    }
}