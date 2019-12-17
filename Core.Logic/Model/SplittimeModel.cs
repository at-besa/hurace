using System;
using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.Logic.Model
{
    public class SplittimeModel : IComparable<SplittimeModel>
    {
        public int RaceDataId { get; set; }
        public int RunNo { get; set; }
        public int SplittimeNo { get; set; }
        public DateTime Time { get; set; }
        
        public override string ToString() =>
            $"Splittime(RaceDataId:{RaceDataId}, RunNo:{RunNo}, SplittimeNo:{SplittimeNo}, Time:{Time})";

        public int CompareTo(SplittimeModel other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var raceDataIdComparison = RaceDataId.CompareTo(other.RaceDataId);
            if (raceDataIdComparison != 0) return raceDataIdComparison;
            var runNoComparison = RunNo.CompareTo(other.RunNo);
            if (runNoComparison != 0) return runNoComparison;
            var splittimeNoComparison = SplittimeNo.CompareTo(other.SplittimeNo);
            if (splittimeNoComparison != 0) return splittimeNoComparison;
            return Time.CompareTo(other.Time);
        }
    }
}