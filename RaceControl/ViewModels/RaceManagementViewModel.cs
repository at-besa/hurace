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
        public ICollection<string> Genders { get; set; } = new List<string>{"m", "f"};

        public RaceManagementViewModel()
        {
	        GetRaces();
            GetRaceTypes();
        }

        private async void GetRaceTypes()
        {
	        logic = new RaceLogic();

            var types = await logic.GetRaceTypes();

	        foreach (var type in types)
	        {
		        RaceTypes.Add(type);
	        }
        }


        private async void GetRaces()
        {
            logic = new RaceLogic();

            var raceModels = await logic.GetRaces();

            foreach (var raceModel in raceModels)
            {
                var raceViewModel = new RaceViewModel(raceModel); 
                RaceViewModels.Add(raceViewModel);
            }

        }
    }

}
