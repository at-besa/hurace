using Hurace.Core.Logic.Model;
using Swack.UI.ViewModels;

namespace RaceControl.ViewModels.RaceManagementHelpers
{
    public class RaceViewModel : NotifyPropertyChanged
    {
        public RaceModel RaceModel { get; set; }
        public string Image { get; set; }
        public bool NewRace { get; set; } = false;


        public RaceViewModel(RaceModel raceModelModel, bool newRace = false)
        {
            RaceModel = raceModelModel;
            NewRace = newRace;

            if (RaceModel.Sex == "m")
            {
                Image = "/Images/mars.png";
            }
            else
            {
                Image = "/Images/venus.png";
            }
        }
    }
}
