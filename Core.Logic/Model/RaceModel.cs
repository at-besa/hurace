using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic.Helpers;

namespace Hurace.Core.Logic.Model
{
    public class RaceModel : NotifyPropertyChanged, IComparable<RaceModel> 
    {
        private Status status;
        private string name;

        public RaceModel(Race race)
        {
            Id = race.Id;
            Type = race.Type;
            Status = race.Status;
            Date = race.Date;
            Name = race.Name;
            Location = race.Location;
            Splittimes = race.Splittimes;
            Sex = race.Sex;
        }

        public RaceModel(){}

        public int Id { get; set; }
        public RaceType Type { get; set; }

        public Status Status
        {
            get => status;
            set => Set(ref status, value);
        }

        public int ActualRun { get; set; } = 1;
        public DateTime Date { get; set; }

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        public string Location { get; set; }
        public int Splittimes { get; set; }
        public string Sex { get; set;  }

        public Race ToRace()
        {
            return new Race
            {
                Id = Id,
                Type = Type,
                Status = Status,
                Date = Date,
                Name = Name,
                Location = Location,
                Splittimes = Splittimes,
                Sex = Sex
            };
        }
        
        public override string ToString() =>
            $"RaceId(id:{Id}, Type:{Type}, Name:{Name}, Location:{Location}, Sex:{Sex}, Date:{Date:yyyy-MM-dd})";

        public int CompareTo(RaceModel other) {
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