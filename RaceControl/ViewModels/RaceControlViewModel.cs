using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using Swack.UI.ViewModels;

namespace RaceControl.ViewModels
{
    public class RaceControlViewModel : NotifyPropertyChanged
    {
        private readonly RaceControlLogic raceControlLogic = RaceControlLogic.Instance;
        private readonly RaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;
        public ObservableCollection<RaceModel> Source { get; } = new ObservableCollection<RaceModel>();

        private StartListMemberModel selectedSkierViewModel;
        public StartListMemberModel SelectedSkierViewModel
        {
	        get => selectedSkierViewModel;
	        set => Set(ref selectedSkierViewModel, value);
        }

        public CommandBase StartRunCommand { get; set; }
        public CommandBase ClearanceCommand { get; set; }

        private RaceControlModel raceControlModel;
        public RaceControlModel RaceControlModel
        {
	        get => raceControlModel;
	        set => Set(ref raceControlModel, value);
        }

        public RaceControlViewModel()
        {
            Init();
            StartRunCommand = new CommandBase(StartRun);
            ClearanceCommand = new CommandBase(Clearance);
        }

        private void StartRun(object sender, EventArgs e)
        {
            if (SelectedSkierViewModel != null)
	        {
		        SelectedSkierViewModel.Blocked = false;
            }
        }
        private void Clearance(object sender, EventArgs e)
        {
            if (SelectedSkierViewModel != null)
	        {
		        SelectedSkierViewModel.Finished = true;
	        }
	    }


        private async void Init()
        {
            await LoadDataAsync();
        }


        private async Task LoadDataAsync()
        {
            Source.Clear();
            var race = await raceManagementLogic.GetRunningRace();
            RaceControlModel = await raceControlLogic.GetRaceControlForRaceId(race.Id);
        }

    }
}
