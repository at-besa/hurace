using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using RaceControl.Helpers;

namespace RaceControl.ViewModels
{
    public class StartListViewModel 
    {
	    public ObservableCollection<RaceViewModel> Races { get; set; } = new ObservableCollection<RaceViewModel>();
	    public RaceViewModel SelectedRace { get; set; }

	    private RaceLogic logic;

	    public StartListViewModel()
	    {
		    ShowRaces();
	    }


	    private async void ShowRaces()
	    {
		    logic = new RaceLogic();

		    var raceModels = await logic.GetRaces();

		    foreach (var raceModel in raceModels)
		    {
			    var raceViewModel = new RaceViewModel(raceModel);
			    Races.Add(raceViewModel);
		    }
	    }
	}
}
