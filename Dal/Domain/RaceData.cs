using System;

namespace Hurace.Dal.Domain
{
    // DTO/Domain Object
    public class RaceData
    {
        private const int Splittimes = 5;
        public Race Race { get; set; }
        public int SkierId { get; set; }
        public int Run { get; set; }
        public DateTime[] SplitTime { get; set; } = new DateTime[Splittimes];
        public DateTime FinishTime { get; set; }
        public bool Disqualified { get; set; }
        
        public override string ToString() =>
            $"RaceData(id:{SkierId}, Race:{Race}, Run:{Run}, SplitTime:{SplitTime}, FinishTime:{FinishTime}, Disqualified:{Disqualified})";
    }
}