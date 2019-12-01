using System;
using System.Text;
using Hurace.Core.Logic.Model;

namespace RaceControl.ViewModels
{
    public class RaceViewModel : NotifyPropertyChanged
    {
        public RaceModel Race { get; set; }
        public string Image { get; set; }

        public RaceViewModel(RaceModel raceModel)
        {
            Race = raceModel;

            if (Race.Race.Sex == "m")
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
