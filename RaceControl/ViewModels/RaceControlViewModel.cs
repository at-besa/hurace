using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using NotifyPropertyChanged = RaceControl.Helpers.NotifyPropertyChanged;

namespace RaceControl.ViewModels
{
    public class RaceControlViewModel : NotifyPropertyChanged
    {
        private readonly IRaceControlLogic raceControlLogic = RaceControlLogic.Instance;
        private readonly IRaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;

        private ICollection<SplitTimeModel> actualSplittimes;
        public ICollection<SplitTimeModel> ActualSplittimes
        {
	        get => actualSplittimes;
	        set => Set(ref actualSplittimes, value);
        }

        private ICollection<SplitTimeModel> lastSplittimes;
        public ICollection<SplitTimeModel> LastSplittimes
        {
	        get => lastSplittimes;
	        set => Set(ref lastSplittimes, value);
        }

        public bool SimulatorOnOff
        {
	        get => simulatorOnOff;
	        set => Set(ref simulatorOnOff, value);
        }


        private RaceControlModel raceControlModel;
        public RaceControlModel RaceControlModel
        {
	        get => raceControlModel;
	        set => Set(ref raceControlModel, value);
        }

        private StartListMemberModel selectedSkierViewModel;
        public StartListMemberModel SelectedSkierViewModel {
	        get => selectedSkierViewModel;
	        set  {
				Set(ref selectedSkierViewModel, value);

				SelectedSkierBoxVisible = Visibility.Visible;

				StartRunCommand.IsExecutionPossible = true;
				ClearanceCommand.IsExecutionPossible = true;
				SimulatorOnOffCommand.IsExecutionPossible = true;
                ShowSplittimesForActualSkier();
	        }
        }

        private Visibility selectedSkierBoxVisible = Visibility.Collapsed;
        public Visibility SelectedSkierBoxVisible
        {
	        get => selectedSkierBoxVisible;
	        set => Set(ref selectedSkierBoxVisible, value);
        }

        private Visibility lastSkierBoxVisible = Visibility.Collapsed;
        public Visibility LastSkierBoxVisible {
	        get => lastSkierBoxVisible;
	        set => Set(ref lastSkierBoxVisible, value);
        }


        private StartListMemberModel lastSkierViewModel;
        private bool simulatorOnOff;

        public StartListMemberModel LastSkierViewModel {
	        get => lastSkierViewModel;
	        set {
		        Set(ref lastSkierViewModel, value);
	        }
        }

        public CommandBase StartRunCommand { get; set; }
        public CommandBase ClearanceCommand { get; set; }
        public CommandBase DisqualifyCommand { get; set; }
        public CommandBase SimulatorOnOffCommand { get; set; }

        public RaceControlViewModel()
        {
            Init();
            StartRunCommand = new CommandBase(StartRun);
            ClearanceCommand = new CommandBase(Clearance);
            DisqualifyCommand = new CommandBase(Disqualify);
            SimulatorOnOffCommand = new CommandBase(SetSimulator);

            StartRunCommand.IsExecutionPossible = false;
            ClearanceCommand.IsExecutionPossible = false;
            SimulatorOnOffCommand.IsExecutionPossible = false;
        }

        private async void Init()
        {
	        await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
	        var race = await raceManagementLogic.GetRunningRace();
	        RaceControlModel = await raceControlLogic.GetRaceControlForRaceId(race.Id);
        }

        private async void StartRun(object sender, EventArgs e)
        {
            if (SelectedSkierViewModel != null)
            {
                await raceControlLogic.StartRun(SelectedSkierViewModel, RaceControlModel.StartListModel.raceId);
            }
        }

        private async void Disqualify(object sender, EventArgs e)
        {
	        if (SelectedSkierViewModel != null)
	        {
		        await raceControlLogic.Disqualify(SelectedSkierViewModel, RaceControlModel.StartListModel.raceId);
	        }
        }

        private async void SetSimulator(object sender, EventArgs e)
        {
	        if (SelectedSkierViewModel != null)
	        {
		        SimulatorOnOff = !SimulatorOnOff;
                await raceControlLogic.SimulatorOnOff(SimulatorOnOff, RaceControlModel.RaceModel.Id);
	        }
        }

        private async void Clearance(object sender, EventArgs e)
        {
	        if ((ActualSplittimes.Count >= RaceControlModel.RaceModel.Splittimes && !SelectedSkierViewModel.Blocked) 
	            || SelectedSkierViewModel.Disqualified)
	        {

		        LastSkierBoxVisible = Visibility.Visible;

                await raceControlLogic.Clearance(SelectedSkierViewModel, RaceControlModel.StartListModel.raceId);
                LastSkierViewModel = SelectedSkierViewModel;
		        SelectedSkierViewModel = RaceControlModel.StartListModel.StartListMembers.FirstOrDefault(model =>
			        model.Startposition == SelectedSkierViewModel.Startposition + 1);

		        LastSplittimes = ActualSplittimes;
		        ShowSplittimesForActualSkier();

		        //if (SelectedSkierViewModel != null)
		        //{
			       // SelectedSkierViewModel.Finished = true;
		        //}
            }
	    }

        private async void ShowSplittimesForActualSkier()
        {
	        if (SelectedSkierViewModel != null)
		        ActualSplittimes = await raceControlLogic.GetSplittimesForSkier(SelectedSkierViewModel.Skier.Id, 1);
        }


    }
}
