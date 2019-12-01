using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;

namespace RaceControl.ViewModels
{
    public class RaceManagementViewModel : Observable
    {
        public ObservableCollection<RaceViewModel> RaceViewModels { get; set; } = new ObservableCollection<RaceViewModel>();
        public RaceViewModel SelectedRaceViewModel { get; set; }
         
        private RaceLogic logic;

        public RaceManagementViewModel()
        {
            getmystuff();

        }


        private async void getmystuff()
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
