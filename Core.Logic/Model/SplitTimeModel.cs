using System;
using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.Logic.Model
{
    public class SplitTimeModel : IComparable<SplitTimeModel>
    {
        public int RaceDataId { get; set; }
        public int RunNo { get; set; }
        public int SplitTimeNo { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan TimeOffsetToWinner { get; set; }

        public override string ToString() =>
            $"SplitTime(RaceDataId:{RaceDataId}, RunNo:{RunNo}, SplitTimeNo:{SplitTimeNo}, Time:{Time})";

        public SplitTime ToSplitTime()
        {
	        return new SplitTime
	        {
		        RaceDataId = RaceDataId,
		        RunNo = RunNo,
		        SplittimeNo = SplitTimeNo,
		        Time = Time
	        };

        }

        public int CompareTo(SplitTimeModel other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var raceDataIdComparison = RaceDataId.CompareTo(other.RaceDataId);
            if (raceDataIdComparison != 0) return raceDataIdComparison;
            var runNoComparison = RunNo.CompareTo(other.RunNo);
            if (runNoComparison != 0) return runNoComparison;
            var splittimeNoComparison = SplitTimeNo.CompareTo(other.SplitTimeNo);
            if (splittimeNoComparison != 0) return splittimeNoComparison;
            return Time.CompareTo(other.Time);
        }
    }
}