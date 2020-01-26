using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hurace.Core.Logic.Annotations;
using Hurace.Core.Logic.Helpers;

namespace Hurace.Core.Logic.Model
{
    public class StartListModel : NotifyPropertyChanged
    {
	    private ICollection<StartListMemberModel> startListMembers;
	    public int raceId { get; set; }

	    public ICollection<StartListMemberModel> StartListMembers
	    {
		    get => startListMembers;
		    set => Set(ref startListMembers, value);
	    }
    }
}