using System;

namespace Hurace.Core.DAL.Domain
{
    public class Run : IComparable<Run>
    {
        public int RaceId { get; set; }
        public int RunNo { get; set; }
        public bool Running { get; set; }
        public bool Finished { get; set; }
        
        public override string ToString() =>
            $"Run(RaceId:{RaceId}, RunNo:{RunNo}, Running:{Running}, Finished:{Finished})";

        public int CompareTo(Run other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var raceIdComparison = RaceId.CompareTo(other.RaceId);
            if (raceIdComparison != 0) return raceIdComparison;
            var runNoComparison = RunNo.CompareTo(other.RunNo);
            if (runNoComparison != 0) return runNoComparison;
            var runningComparison = Running.CompareTo(other.Running);
            if (runningComparison != 0) return runningComparison;
            return Finished.CompareTo(other.Finished);
        }
    }
}