using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;

namespace RaceControl.ViewModels
{
    public class ScreenControlViewModel : NotifyPropertyChanged
	{

	    private readonly IRaceControlLogic raceControlLogic = RaceControlLogic.Instance;
	    private readonly IRaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;

	    private ICollection<SplitTimeModel> actualSplittimes;
	    public ICollection<SplitTimeModel> ActualSplittimes {
		    get => actualSplittimes;
		    set => Set(ref actualSplittimes, value);
	    }


	    private RaceControlModel raceControlModel;
	    public RaceControlModel RaceControlModel {
		    get => raceControlModel;
		    set => Set(ref raceControlModel, value);
		}

	    private Visibility selectedSkierBoxVisible = Visibility.Collapsed;
	    public Visibility SelectedSkierBoxVisible {
		    get => selectedSkierBoxVisible;
		    set => Set(ref selectedSkierBoxVisible, value);
	    }

		private StartListMemberModel selectedSkierViewModel;
	    public StartListMemberModel SelectedSkierViewModel {
		    get => selectedSkierViewModel;
		    set {
			    Set(ref selectedSkierViewModel, value);

			    SelectedSkierBoxVisible = Visibility.Visible;

			    ShowSplittimesForActualSkier();
		    }
		}
	    public CommandBase OpenScreenCommand { get; set; }
	    public CommandBase ClearanceCommand { get; set; }
	    public CommandBase DisqualifyCommand { get; set; }
	    public CommandBase SimulatorOnOffCommand { get; set; }

		public ScreenControlViewModel()
	    {
			Init();
			OpenScreenCommand = new CommandBase(OpenScreen);

		}
		private void OpenScreen(object sender, EventArgs e)
		{
			
		}

		private async void Init()
	    {
		    await LoadDataAsync();
	    }

	    private async Task LoadDataAsync()
	    {
		    var race = await raceManagementLogic.GetRunningRace();
		    RaceControlModel = await raceControlLogic.GetRaceControlForRaceId(race.Id, 1);

	    }



	    private async void ShowSplittimesForActualSkier()
	    {
		    if (SelectedSkierViewModel != null)
			    ActualSplittimes = await raceControlLogic.GetSplittimesForSkier(SelectedSkierViewModel.Skier.Id, 1);
	    }


	}
}
