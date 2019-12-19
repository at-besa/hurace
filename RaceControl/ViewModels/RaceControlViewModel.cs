﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using Swack.UI.ViewModels;

namespace RaceControl.ViewModels
{
    public class RaceControlViewModel : NotifyPropertyChanged
    {
        private readonly IRaceControlLogic raceControlLogic = RaceControlLogic.Instance;
        private readonly IRaceManagementLogic raceManagementLogic = RaceManagementManagementLogic.Instance;

        private ICollection<SplittimeModel> actualSplittimes;
        public ICollection<SplittimeModel> ActualSplittimes
        {
	        get => actualSplittimes;
	        set => Set(ref actualSplittimes, value);
        }

        private ICollection<SplittimeModel> lastSplittimes;
        public ICollection<SplittimeModel> LastSplittimes
        {
	        get => lastSplittimes;
	        set => Set(ref lastSplittimes, value);
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

				StartRunCommand.IsExecutionPossible = true;
				ClearanceCommand.IsExecutionPossible = true;
                ShowSplittimesForActualSkier();
	        }
        }

        private StartListMemberModel lastSkierViewModel;
        public StartListMemberModel LastSkierViewModel {
	        get => lastSkierViewModel;
	        set {
		        Set(ref lastSkierViewModel, value);
	        }
        }

        public CommandBase StartRunCommand { get; set; }
        public CommandBase ClearanceCommand { get; set; }

        public RaceControlViewModel()
        {
            Init();
            StartRunCommand = new CommandBase(StartRun);
            ClearanceCommand = new CommandBase(Clearance);

            StartRunCommand.IsExecutionPossible = false;
            ClearanceCommand.IsExecutionPossible = false;
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
                //SelectedSkierViewModel.Blocked = false;
                await raceControlLogic.StartRun(SelectedSkierViewModel, RaceControlModel.StartListModel.raceId);
            }
        }
        
        private async void Clearance(object sender, EventArgs e)
        {
	        if ((ActualSplittimes.Count >= RaceControlModel.RaceModel.Splittimes && !SelectedSkierViewModel.Blocked) 
	            || SelectedSkierViewModel.Disqualified)
	        {
		        LastSkierViewModel = SelectedSkierViewModel;
		        SelectedSkierViewModel = RaceControlModel.StartListModel.StartListMembers.FirstOrDefault(model =>
			        model.Startposition == SelectedSkierViewModel.Startposition + 1);

		        LastSplittimes = ActualSplittimes;
		        ShowSplittimesForActualSkier();

		        if (SelectedSkierViewModel != null)
		        {
			        SelectedSkierViewModel.Finished = true;
		        }
            }
	    }

        private async void ShowSplittimesForActualSkier()
        {
	        if (SelectedSkierViewModel != null)
		        ActualSplittimes = await raceControlLogic.GetSplittimesForSkier(SelectedSkierViewModel.Skier.Id, 1);
        }


    }
}
