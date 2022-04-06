namespace SSIW2_CSP.Controllers
{
    public interface IController<T> where T : struct
    {
        void FindSolutions();
        void PrintResult();
    }
}
