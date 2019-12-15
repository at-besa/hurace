using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hurace.Core.Logic.Annotations;

namespace Hurace.Core.Logic.Model
{
    public class StartListModel : IComparable<StartListModel>, INotifyPropertyChanged
    {
        public int raceId { get; set; }

        public ICollection<StartListMemberModel> StartListMembers { get; set; }

        public int CompareTo(StartListModel other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return raceId.CompareTo(other.raceId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}