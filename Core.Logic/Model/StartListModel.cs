using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hurace.Core.Logic.Annotations;

namespace Hurace.Core.Logic.Model
{
    public class StartListModel
    {
        public int raceId { get; set; }

        public ICollection<StartListMemberModel> StartListMembers { get; set; }

    }
}