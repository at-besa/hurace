using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.Logic.Model
{
    public class RaceModel : IComparable<RaceModel>
    {
        public Race Race { get; set; }

        public int CompareTo(RaceModel other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Comparer<Race>.Default.Compare(Race, other.Race);
        }
    }
}