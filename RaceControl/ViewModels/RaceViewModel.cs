using System;
using System.Text;
using Hurace.Core.Logic.Model;

namespace RaceControl.ViewModels
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

            if (RaceModel.Race.Sex == "m")
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
