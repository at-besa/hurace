using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;

namespace RaceControl.ViewModels
{
    public class RaceManagementViewModel
    {
        public ObservableCollection<RaceViewModel> RaceViewModels { get; set; } = new ObservableCollection<RaceViewModel>();
        public RaceViewModel SelectedRaceViewModel { get; set; }
         
        private RaceLogic logic;

        public ObservableCollection<string> RaceTypes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> RaceStates { get; set; } = new ObservableCollection<string>();
        public ICollection<string> Genders { get; set; } = new List<string>{"m", "f"};

        public RaceManagementViewModel()
        {
	        logic = new RaceLogic();

            GetRaces();
            GetRaceTypes();
            GetRaceStates();
        }

        private async void GetRaceTypes()
        {
            var types = await logic.GetRaceTypes();

	        foreach (var type in types)
	        {
		        RaceTypes.Add(type);
	        }
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
        
        private async void GetRaceStates()
        {
	        var raceStates = await logic.GetRaceStates();

	        foreach (var state in raceStates)
	        {
		        RaceStates.Add(state);
	        }

        }
    }

}
