using System.Collections.Generic;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Domains;

namespace SSIW2_CSP.Labels
{
    public class LabelWithDeletedValues<T>: ILabel<T> where T: struct
    {
        public int ID { get; private init; }
        public ILabel<T> Label { get; }
            
        public T? Value
        {
            get => Label.Value;
            set => Label.Value = value;
        }
        public IDomain<T> Domain => Label.Domain;

        public List<T> FreeDomainValues => Label.FreeDomainValues;
        public List<IConstraint> Constraints => Label.Constraints;

        public Dictionary<int, List<T>> RemovedValues = new();
        public LabelWithDeletedValues(ILabel<T> label, int id)
        {
            Label = label;
            ID = id;
        }
    }
}