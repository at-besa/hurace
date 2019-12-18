using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.Logic.Model
{
    public class RaceControlModel : IComparable<RaceControlModel>
    {
        public RaceModel RaceModel { get; set; }
        public SkierModel SkierModel { get; set; }
        public StartListModel StartListModel { get; set; }


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