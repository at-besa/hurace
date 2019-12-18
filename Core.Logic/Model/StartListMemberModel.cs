using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Logic.Model
{
    public class StartListMemberModel : IComparable<StartListMemberModel> 
    {
        public SkierModel Skier { get; set; }
        public int Startposition { get; set; }
        
        public bool Disqualified { get; set; }
        public bool Running { get; set; }
        public bool Blocked { get; set; }
        public bool Finished { get; set; }

        public int CompareTo(StartListMemberModel other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var skierComparison = Comparer<SkierModel>.Default.Compare(Skier, other.Skier);
            if (skierComparison != 0) return skierComparison;
            var startpositionComparison = Startposition.CompareTo(other.Startposition);
            if (startpositionComparison != 0) return startpositionComparison;
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
