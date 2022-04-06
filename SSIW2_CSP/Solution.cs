using SSIW2_CSP.Labels;

namespace SSIW2_CSP
{
    public class Solution<T> where T:struct
    {
        public Solution(T?[] labels, int stepsCounter, int stepsCounterFromLast, int returnsCounter, int returnsCounterFromLast, long elapsedMilliseconds, long lastMillisecondsCounter)
        {
            Labels = labels;
            CurrentStepsNumber = stepsCounter;
            StepsCounterFromLast = stepsCounterFromLast;
            CurrentReturnsNumber = returnsCounter;
            ReturnsCounterFromLast = returnsCounterFromLast;
            CurrentMilliseconds = elapsedMilliseconds;
            MillisecondsFromLast = lastMillisecondsCounter;
        }


        public T?[] Labels { get; }
        public int CurrentStepsNumber { get; }
        public int StepsCounterFromLast { get; }
        public int CurrentReturnsNumber { get; }
        public int ReturnsCounterFromLast { get; }
        public long CurrentMilliseconds { get; }
        public long MillisecondsFromLast { get; }
    }
}