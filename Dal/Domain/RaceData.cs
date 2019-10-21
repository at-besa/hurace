using System;

namespace Hurace.Dal.Domain
{
    // DTO/Domain Object
    public class RaceData
    {
        public Race Race { get; set; }
        public int SkierId { get; set; }
        public Array[] SplitTime { get; set; }
        public bool Disqualified { get; set; }
        
        public override string ToString() =>
            $"RaceData(id:{SkierId}, Race:{Race}, SplitTime:{SplitTime}, Disqualified:{Disqualified})";
    }
}