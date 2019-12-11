using System;
using System.Collections.Generic;

namespace Hurace.Core.Logic.Model
{
    public class StartListModel
    {
        public int raceId { get; set; }
        public ICollection<StartListMemberModel> StartListMembers { get; set; }

    }
}