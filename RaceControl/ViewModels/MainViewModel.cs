using System;
using System.Collections.Generic;
using System.Text;
using Hurace.Core.DAL.Domain;

namespace RaceControl.ViewModels
{
    partial class MainViewModel
    {
        public IEnumerable<NavigationModel> NavigationList { get; set; }
        public NavigationModel NavitationStart { get; set; }      
        public MainViewModel()
        {
            NavitationStart = new NavigationModel("Races");
            NavigationList = new List<NavigationModel>()
            {
                NavitationStart, 
                new NavigationModel("Start List"),
                new NavigationModel("Race Control"),
                new NavigationModel("Screen Control")
            };

        }
    }


}
