using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Hurace.Core.Logic.Helpers;

namespace Hurace.Core.Logic.Model
{
    public class StartListMemberModel : NotifyPropertyChanged, IComparable<StartListMemberModel>
    {
        private bool blocked;
        private bool running;
        private bool disqualified;
        private int startposition;
        private bool finished;
        private int runNo;
        private SkierModel skier;


        public int Startposition
        {
            get => startposition;
            set => Set(ref startposition, value);
        }

        public bool Disqualified
        {
            get => disqualified;
            set => Set(ref disqualified, value);
        }

        public bool Running
        {
            get => running;
            set => Set(ref running, value);
        }

        public bool Blocked
        {
            get => blocked;
            set => Set(ref blocked, value);
        }

        public bool Finished
        {
            get => finished;
            set => Set(ref finished, value);
        }

        public int RunNo
        {
            get => runNo;
            set => Set(ref runNo, value);
        }

        public SkierModel Skier
        {
            get => skier;
            set => Set(ref skier, value);
        }

        private ICommand _deleteButtonCommand;
        public ICommand DeleteButtonCommand
        {
            get => _deleteButtonCommand;
            set => Set(ref _deleteButtonCommand, value);
        }

        public int CompareTo(StartListMemberModel other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var skierComparison = Comparer<SkierModel>.Default.Compare(Skier, other.Skier);
            if (skierComparison != 0) return skierComparison;
            var startpositionComparison = Startposition.CompareTo(other.Startposition);
            if (startpositionComparison != 0) return startpositionComparison;
            var disqualifiedComparison = Disqualified.CompareTo(other.Disqualified);
            if (disqualifiedComparison != 0) return disqualifiedComparison;
            var runningComparison = Running.CompareTo(other.Running);
            if (runningComparison != 0) return runningComparison;
            var blockedComparison = Blocked.CompareTo(other.Blocked);
            if (blockedComparison != 0) return blockedComparison;
            var runNoComparison = RunNo.CompareTo(other.RunNo);
            if (runNoComparison != 0) return runNoComparison;
            return Finished.CompareTo(other.Finished);
        }
    }
}
