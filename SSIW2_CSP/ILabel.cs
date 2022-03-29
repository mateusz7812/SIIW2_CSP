namespace SSIW2_CSP
{
    interface ILabel<T> where T : struct
    {
        public T? Value { get; set; }
        public IDomain<T> Domain { get; }
    }
}
