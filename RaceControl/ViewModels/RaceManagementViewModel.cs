using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoSimulationPlcSimAdv.Commands;
using Hurace.Core.Logic;

namespace RaceControl.ViewModels
{
    public class RaceManagementViewModel
    {
	    private RaceLogic logic;
        public ObservableCollection<RaceViewModel> RaceViewModels { get; set; } = new ObservableCollection<RaceViewModel>();
        public RaceViewModel SelectedRaceViewModel { get; set; }
        public ObservableCollection<string> RaceTypes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> RaceStates { get; set; } = new ObservableCollection<string>();
        public ICollection<string> Genders { get; set; } = new List<string>{"m", "f"};

        public CommandBase SaveCommand { get; set; }
        public CommandBase DeleteCommand { get; set; }

        public RaceManagementViewModel()
        {
	        logic = new RaceLogic();

            GetRaces();
            GetRaceTypes();
            GetRaceStates();
            
            SaveCommand = new CommandBase(SaveRace);
            DeleteCommand = new CommandBase(DeleteRace);
            
        }

        private async void GetRaces()
        {
	        var raceModels = await logic.GetRaces();

	        foreach (var raceModel in raceModels)
	        {
		        var raceViewModel = new RaceViewModel(raceModel);
		        RaceViewModels.Add(raceViewModel);
	        }

        }

        private async void GetRaceTypes()
        {
            var types = await logic.GetRaceTypes();

	        foreach (var type in types)
	        {
		        RaceTypes.Add(type);
	        }
        }

        private async void GetRaceStates()
        {
	        RaceStates.Clear();
	        var raceStates = await logic.GetRaceStates();

	        foreach (var state in raceStates)
	        {
		        RaceStates.Add(state);
	        }
        }

        private async void SaveRace(object sender, EventArgs eventArgs)
        {
	        await logic.SaveRace(SelectedRaceViewModel.RaceModel);
        }
        
        private async void DeleteRace(object sender, EventArgs e)
        {
	        await logic.DeleteRace(SelectedRaceViewModel.RaceModel.Race.Id);
	        RaceViewModels.Remove(SelectedRaceViewModel);
	        SelectedRaceViewModel = RaceViewModels[0];
        }
    }

}
