namespace SSIW2_CSP
{
    interface IController<T> where T : struct
    {
        void FindSolutions();
    }
}
