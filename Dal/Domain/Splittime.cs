using System;

namespace Hurace.Dal.Domain
{
    // DTO/Domain Object
    public class Splittime
    {
        public int RaceDataId { get; set; }
        public int RunNo { get; set; }
        public int SplittimeNo { get; set; }
        public DateTime Time { get; set; }
        public override string ToString() =>
            $"Splittime(RaceDataId:{RaceDataId}, RunNo:{RunNo}, SplittimeNo:{SplittimeNo}, Time:{Time})";
    }
}