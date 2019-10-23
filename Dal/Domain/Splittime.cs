using System;

namespace Hurace.Dal.Domain
{
    // DTO/Domain Object
    public class Splittime
    {
        private const int Splittimes = 5;
        public int Run { get; set; }
        public DateTime[] SplitTime { get; set; } = new DateTime[Splittimes];
        public DateTime FinishTime { get; set; }
        public override string ToString() =>
            $"RaceData(Run:{Run}, SplitTime:{SplitTime}, FinishTime:{FinishTime})";
    }
}