using System;

namespace Hurace.Dal.Domain
{
    // DTO/Domain Object
    public class RaceData
    {
        private const int Splittimes = 5;
        public Race Race { get; set; }
        public int SkierId { get; set; }
        public string[] SplitTime { get; set; } = new string[Splittimes];
        public bool Disqualified { get; set; }
        
        public override string ToString() =>
            $"RaceData(id:{SkierId}, Race:{Race}, SplitTime:{SplitTime}, Disqualified:{Disqualified})";
    }
}