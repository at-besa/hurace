using System;
using System.Collections.Generic;

namespace Hurace.Core.DAL.Domain
{
    // DTO/Domain Object
    public class StartList : IComparable<StartList>
    {
        public Race Race { get; set; }
        public int SkierId { get; set; }
        public int StartPos { get; set; }

        public override string ToString() =>
            $"StartList(raceId: {Race.Id}, Name:{Race.Name}, skierId:{SkierId}, StartPos:{StartPos})";

        public int CompareTo(StartList other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var raceComparison = Comparer<Race>.Default.Compare(Race, other.Race);
            if (raceComparison != 0) return raceComparison;
            var skierIdComparison = SkierId.CompareTo(other.SkierId);
            if (skierIdComparison != 0) return skierIdComparison;
            return StartPos.CompareTo(other.StartPos);
        }
    }
}