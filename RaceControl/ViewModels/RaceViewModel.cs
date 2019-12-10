using System;
using System.Text;
using Hurace.Core.Logic.Model;

namespace RaceControl.ViewModels
{
    public class RaceViewModel : NotifyPropertyChanged
    {
        public RaceModel RaceModel { get; set; }
        public string Image { get; set; }

        public RaceViewModel(RaceModel raceModelModel)
        {
            RaceModel = raceModelModel;

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
