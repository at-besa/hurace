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
        public Race Race { get; set; }
        public SkierModel SkierModel { get; set; }
        
        
        public int CompareTo(RaceControlModel other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Comparer<Race>.Default.Compare(Race, other.Race);
        }
    }
}