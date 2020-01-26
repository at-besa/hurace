using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic.Helpers;

namespace Hurace.Core.Logic.Model
{
    public class RaceControlModel : NotifyPropertyChanged, IComparable<RaceControlModel>
    {
	    private StartListModel startListModel;
	    private RaceModel raceModel;

	    public RaceModel RaceModel
	    {
		    get => raceModel;
		    set => Set(ref  raceModel, value);
	    }

	    public SkierModel SkierModel { get; set; }

        public StartListModel StartListModel
        {
	        get => startListModel;
	        set => Set(ref startListModel, value);
        }


        public int CompareTo(RaceControlModel other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var raceModelComparison = Comparer<RaceModel>.Default.Compare(RaceModel, other.RaceModel);
            if (raceModelComparison != 0) return raceModelComparison;
            var skierModelComparison = Comparer<SkierModel>.Default.Compare(SkierModel, other.SkierModel);
            if (skierModelComparison != 0) return skierModelComparison;
            return Comparer<StartListModel>.Default.Compare(StartListModel, other.StartListModel);
        }
    }
}