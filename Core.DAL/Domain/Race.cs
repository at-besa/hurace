using System;
using System.Collections.Generic;

namespace Hurace.Core.DAL.Domain
{

    // DTO/Domain Object
    public class Race : IComparable<Race>
    {
        public int Id { get; set; }
        public RaceType Type { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Splittimes { get; set; }
        public string Sex { get; set;  }

        public override string ToString() =>
            $"RaceId(id:{Id}, Type:{Type}, Name:{Name}, Location:{Location}, Sex:{Sex}, Date:{Date:yyyy-MM-dd})";

        public int CompareTo(Race other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0) return idComparison;
            var typeComparison = Comparer<RaceType>.Default.Compare(Type, other.Type);
            if (typeComparison != 0) return typeComparison;
            var statusComparison = Comparer<Status>.Default.Compare(Status, other.Status);
            if (statusComparison != 0) return statusComparison;
            var dateComparison = Date.CompareTo(other.Date);
            if (dateComparison != 0) return dateComparison;
            var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
            if (nameComparison != 0) return nameComparison;
            var locationComparison = string.Compare(Location, other.Location, StringComparison.Ordinal);
            if (locationComparison != 0) return locationComparison;
            var splittimesComparison = Splittimes.CompareTo(other.Splittimes);
            if (splittimesComparison != 0) return splittimesComparison;
            return string.Compare(Sex, other.Sex, StringComparison.Ordinal);
        }
    }
}