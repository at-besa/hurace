using System;

namespace Hurace.Core.DAL.Domain
{

//        Downhill,
//        SuperG,
//        Slalom,
//        GiantSlalom,
//        AlpineCombined

    // DTO/Domain Object
    public class RaceType : IComparable<RaceType>, IComparable
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int NumberOfRuns { get; set; }

        public override string ToString() =>
            $"RaceId(id:{Id}, Type:{Type}, NumberOfRuns:{NumberOfRuns})";

        public int CompareTo(object? obj) {
            return CompareTo((RaceType) obj);
        }

        public int CompareTo(RaceType other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0) return idComparison;
            var typeComparison = string.Compare(Type, other.Type, StringComparison.Ordinal);
            if (typeComparison != 0) return typeComparison;
            return NumberOfRuns.CompareTo(other.NumberOfRuns);
        }
    }
}