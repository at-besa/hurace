using System;
using System.Collections.Generic;

namespace Hurace.Core.Logic.Model
{
    public class StartListModel : IComparable<StartListModel>
    {
        public int raceId { get; set; }
        public ICollection<StartListMemberModel> StartListMembers { get; set; }

        public int CompareTo(StartListModel other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return raceId.CompareTo(other.raceId);
        }
    }
}