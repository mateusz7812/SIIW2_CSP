namespace SSIW2_CSP.Controllers
{
    interface IController<T> where T : struct
    {
        void FindSolutions();
    }
}
