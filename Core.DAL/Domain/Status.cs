using System;

namespace Hurace.Core.DAL.Domain
{
    // DTO/Domain Object
    public class Status : IComparable<Status>, IComparable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString() =>
            $"RaceId(id:{Id}, Name:{Name})";

        public int CompareTo(object? obj) {
            return CompareTo((Status)obj);
        }

        public int CompareTo(Status other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0) return idComparison;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}