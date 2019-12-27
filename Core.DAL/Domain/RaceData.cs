using System;
using System.Collections.Generic;

namespace Hurace.Core.DAL.Domain
{
    // DTO/Domain Object
    public class RaceData : IComparable<RaceData>
    {
        private const string Seperator = " - ";
        private Func<IEnumerable<SplitTime>, string> splitter = splittimes => String.Join(Seperator, splittimes);
        public int Id { get; set; }
        public int RaceId { get; set; }
        public int SkierId { get; set; }
        public bool Disqualified { get; set; }
        public bool Running { get; set; }
        public bool Blocked { get; set; }
        public bool Finished { get; set; }
        public IEnumerable<SplitTime>[] Splittime { get; set; }

        public override string ToString() =>
            $"RaceData(id:{SkierId}, RaceId:{RaceId}, Runs: [1: {splitter(Splittime[0])} 2: {splitter(Splittime[1])}], Disqualified:{Disqualified})";

        public int CompareTo(RaceData other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0) return idComparison;
            var raceIdComparison = RaceId.CompareTo(other.RaceId);
            if (raceIdComparison != 0) return raceIdComparison;
            var skierIdComparison = SkierId.CompareTo(other.SkierId);
            if (skierIdComparison != 0) return skierIdComparison;
            var disqualifiedComparison = Disqualified.CompareTo(other.Disqualified);
            if (disqualifiedComparison != 0) return disqualifiedComparison;
            var runningComparison = Running.CompareTo(other.Running);
            if (runningComparison != 0) return runningComparison;
            var blockedComparison = Blocked.CompareTo(other.Blocked);
            if (blockedComparison != 0) return blockedComparison;
            return Finished.CompareTo(other.Finished);
        }
    }
}